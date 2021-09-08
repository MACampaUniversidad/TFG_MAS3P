using CloudMessageBrokerMicroService.Business.MQTTMessageDefinitions;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudMessageBrokerMicroService.Business.MQTTMessageDissectors
{
    /// <summary>
    /// Los disectores se ocupan de diseccionar un mensaje 
    /// para poder procesarlo extendiendo a la clase MQTTApplicationMessage
    /// de MQTTnet
    /// </summary>
    public sealed class ApplicationMessageReceivedDissector
    {
        public enum EApplicationMessageTopicTypes {

            /// <summary>
            /// Mensajes entrantes desde el broker MQTT de TTN
            /// </summary>
            Up = 10,
            /// <summary>
            /// Mensajes salientes que han sido encolados en el broker de TTN
            /// pero aún no han sido entregados
            /// </summary>
            Queued = 20,
            /// <summary>
            /// Mensajes salientes que han sido enviados al dispositivo final
            /// </summary>
            Sent = 30,
            /// <summary>
            /// Mensajes salientes que han fallado al ser enviados al dispositivo final
            /// </summary>
            Failed = 40,
            /// <summary>
            /// Mensajes de ACK entrantes después de una operación down-link
            /// </summary>
            Ack = 50,
            /// <summary>
            /// Mensajes de ACK entrantes después de una operación down-link
            /// </summary>
            Nack = 60,
            /// <summary>
            /// Mensaje de entrada de un dispositivo nuevo en la red WSN
            /// </summary>
            Join = 70,
            /// <summary>
            /// other - analizar el topic original del mensaje.
            /// </summary>
            Other = 80
        }

        /// <summary>
        /// mensaje original 
        /// </summary>
        public readonly MqttApplicationMessage InitialMessage;
        public readonly EApplicationMessageTopicTypes TopicType;
        public ApplicationMessageReceivedDissector(MqttApplicationMessage message)
        {
            this.InitialMessage = message;

            string[] splittedTopic = message.Topic.Split(@"/");
            switch (splittedTopic[splittedTopic.Length - 1])
            {
                case MQTTTopics.TOPIC_DOWN_ACK:
                    this.TopicType = EApplicationMessageTopicTypes.Ack;
                    break;
                case MQTTTopics.TOPIC_DOWN_FAILED:
                    this.TopicType = EApplicationMessageTopicTypes.Failed;
                    break;
                case MQTTTopics.TOPIC_DOWN_NACK:
                    this.TopicType = EApplicationMessageTopicTypes.Nack;
                    break;
                case MQTTTopics.TOPIC_DOWN_QUEUED:
                    this.TopicType = EApplicationMessageTopicTypes.Queued;
                    break;
                case MQTTTopics.TOPIC_DOWN_SENT:
                    this.TopicType = EApplicationMessageTopicTypes.Sent;
                    break;
                case MQTTTopics.TOPIC_JOIN:
                    this.TopicType = EApplicationMessageTopicTypes.Join;
                    break;
                case MQTTTopics.TOPIC_UP:
                    this.TopicType = EApplicationMessageTopicTypes.Up;
                    break;
                default:
                        this.TopicType = EApplicationMessageTopicTypes.Other;
                    break;
 
            }
        }
    }
}
