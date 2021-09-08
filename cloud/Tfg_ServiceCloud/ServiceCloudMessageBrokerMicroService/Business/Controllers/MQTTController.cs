using CloudMessageBrokerMicroService.Business.MQTTMessageDefinitions;
using CloudMessageBrokerMicroService.Business.MQTTMessageDissectors;
using CloudMessageBrokerMicroService.DataContracts;
using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceCloudMessageBrokerMicroService.DataContracts;
using System;


namespace CloudMessageBrokerMicroService.Business.Controllers
{

    /// <summary>
    /// Gestiona la comunicación con el servidor MQTT - patron singleton
    /// </summary>
    public class MQTTController
    {


        /// <summary>
        /// Singleton en modo "lazy"
        /// </summary>
        private static readonly Lazy<MQTTController> lazyInstance =
            new Lazy<MQTTController>(() => new MQTTController());

        private static readonly object locker = new object(); 
        public static MQTTController Instance { get { return lazyInstance.Value; } }

         

        private IManagedMqttClient mQTTClient;

        private MQTTController()
        {
            //https://github.com/chkr1011/MQTTnet/wiki/ManagedClient#preparation
            //https://dzone.com/articles/mqtt-publishing-and-subscribing-messages-to-mqtt-b
            var options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions
                    (
                       new MqttClientOptionsBuilder()
                            .WithClientId("Service_Cloud")
                            .WithCredentials("mas3-p@ttn", "NNSXS.N75RMIU22BMKX5BPJZLLILIA2DCRP3NCCQQFRQI.73BKS6NENMNHBFC55FLHRRHF7TETYRSTG62EZDCXEBDAUQVWZV7A")
                            .WithTcpServer("eu1.cloud.thethings.network", 1883) //NO TLS (8883)
                            .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
                            .WithRequestProblemInformation(true)
                            .WithRequestResponseInformation(true)
                            .Build()
                    ).Build();
            
            mQTTClient =   new MqttFactory().CreateManagedMqttClient();

            mQTTClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(MQTTTopics.TOPIC_ALL).Build());
            mQTTClient.UseConnectedHandler(x => BrokerStatusController.Instance.NotifyConnectionChange(true));
            mQTTClient.UseDisconnectedHandler(x => BrokerStatusController.Instance.NotifyConnectionChange(false));
            mQTTClient.UseApplicationMessageReceivedHandler(x =>this.receivedMessageHandler(x));
       
            mQTTClient.StartAsync(options);
        }
        /// <summary>
        /// Este método controla la llegada de mensajes procedentes del servidor o brokerMQTT.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private void receivedMessageHandler(MqttApplicationMessageReceivedEventArgs x)
        {
            var messageDissector= new ApplicationMessageReceivedDissector(x.ApplicationMessage);
            lock (locker)
            {
                switch (messageDissector.TopicType)
                {
                    case ApplicationMessageReceivedDissector.EApplicationMessageTopicTypes.Ack:
                        BrokerStatusController.Instance.NotifyProcessedMessage(BrokerStatusController.EDirection.FromServerToBroker);
                        break;
                    case ApplicationMessageReceivedDissector.EApplicationMessageTopicTypes.Nack:
                        BrokerStatusController.Instance.NotifyProcessedMessage(BrokerStatusController.EDirection.FromServerToBroker);
                        break;
                        
                    case ApplicationMessageReceivedDissector.EApplicationMessageTopicTypes.Failed:
                        break;
                    case ApplicationMessageReceivedDissector.EApplicationMessageTopicTypes.Join:
                        BrokerStatusController.Instance.NotifyProcessedMessage(BrokerStatusController.EDirection.FromServerToBroker);
                        break;
                    case ApplicationMessageReceivedDissector.EApplicationMessageTopicTypes.Queued:
                        BrokerStatusController.Instance.NotifyQueuedMessage(BrokerStatusController.EDirection.FromServerToBroker);
                        break;
                    case ApplicationMessageReceivedDissector.EApplicationMessageTopicTypes.Sent:
                        BrokerStatusController.Instance.NotifyProcessedMessage(BrokerStatusController.EDirection.FromServerToBroker);
                        break;
                    case ApplicationMessageReceivedDissector.EApplicationMessageTopicTypes.Up:
                        //entrada de un mensaje desde el broker MQTT. 
                        //https://github.com/Dorin-Mocan/SignalRSwaggerGen/wiki
                        SignalBroadcast(x.ApplicationMessage, true);
                        break;

                }

            }
        }

        private void SignalBroadcast(MqttApplicationMessage applicationMessage, bool v)
        {
            throw new NotImplementedException();
        }

        internal  ActionResult SendMessage(Message message)
        {
            lock (locker)
            {
             var  packet= TFG_MAS3pProtocol.Factories.PacketFactory.CreatePacket(TFG_MAS3pProtocol.Definitions.EMAS3pDirection.CloudToNode,
                     TFG_MAS3pProtocol.Definitions.EMAS3pMessageTypes.Configuration,
                      TFG_MAS3pProtocol.Definitions.EMAS3pActionTypes.Update, message.ClientId);

                try
                {
                    //Verificar que el objeto en el mensaje es realmente un json
                    dynamic serialicedmsg = JObject.Parse(message.JsonObject);
                    //DownLinkMessage downLinkMessage = new MQTTMessageDefinitions.DownLinkMessage
                    //{
                    //    //Los mensajes con alta prioridad requieren de confirmación por parte del cliente
                    //    Confirmed = (message.Priority == EPriority.High),
                    //    Payload_Fields = message.JsonObject// JsonSerializer.Serialize(serialicedmsg, serialicedmsg.GetType())
                    //};
                    mQTTClient.PublishAsync(new MqttApplicationMessageBuilder()
                        .WithTopic($@"v3/mas3-p@ttn/devices/{message.ClientId}/down/push")
                        .WithPayloadFormatIndicator(MQTTnet.Protocol.MqttPayloadFormatIndicator.CharacterData)
                        //.WithPayload("{\"downlinks\": [{\"f_port\": 1,\"frm_payload\": \"vu8=\", \"priority\": \"NORMAL\" }]}")//JsonConvert.SerializeObject(downLinkMessage, Formatting.None))
                         .WithPayload("{\"downlinks\": [{\"f_port\": 1,\"frm_payload\": \""+ packet.GetMessagePayload()+"\", \"priority\": \"NORMAL\" }]}")//JsonConvert.SerializeObject(downLinkMessage, Formatting.None))

                        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                        .WithRetainFlag(true)
                        .Build()
                    );
                }
                catch (JsonReaderException e)
                {
                    return new BadRequestObjectResult(new RequestedOperationFailed { Reason = $@"El cuerpo de mensaje no está correctamente formateado como un objeto JSON : {message.JsonObject}" });
                }
                
                return new OkResult();
            }
           
        }
    }
}
