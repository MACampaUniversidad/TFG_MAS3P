using CloudMessageBrokerMicroService.DataContracts;
using Microsoft.AspNetCore.Mvc;
using ServiceCloudMessageBrokerMicroService.DataContracts;
using System;
using System.Collections.Generic;

namespace CloudMessageBrokerMicroService.Business.Controllers
{

    public class MessageBrokerController : IMessageBrokerController
    {


        /// <summary>
        /// Singleton en modo "lazy" con seguridad frente a concurrencia 
        /// por exclusion mutua.
        /// </summary>
        private static readonly Lazy<MessageBrokerController> lazyInstance =
            new Lazy<MessageBrokerController>(() => new MessageBrokerController());

        private static readonly object locker = new object();

        public static MessageBrokerController Instance { get { return lazyInstance.Value; } }
        private MessageBrokerController()
        {

            serverInputPutStackLP = new Stack<Message>();
            clientInputStackLP = new Stack<Message>();
            serverOutPutStackLP = new Stack<Message>();
            clientOutPutStackLP = new Stack<Message>();
            clientInputStackHP = new Stack<Message>();
            serverOutPutStackHP = new Stack<Message>();
            clientOutPutStackHP = new Stack<Message>();
        }

        /// <summary>
        /// Pila que almacena los mensajes que entran 
        /// desde los clientes del API 
        /// BAJA PRIORIDAD
        /// </summary>
        private Stack<Message> clientInputStackLP;

        /// <summary>
        /// Pila que almacena los mensajes que deben ser
        /// enviados a los clientes del API
        /// BAJA PRIORIDAD
        /// </summary>
        private Stack<Message> clientOutPutStackLP;

        /// <summary>
        /// Pila que almacena los mensajes entrantes desde el servidor MQTT
        /// BAJA PRIORIDAD
        /// </summary>
        private Stack<Message> serverInputPutStackLP;



        /// <summary>
        /// Pila que almacena los mensajes salientes hacia el servidor MQTT
        /// BAJA PRIORIDAD
        /// </summary>
        private Stack<Message> serverOutPutStackLP;

        /// <summary>
        /// Pila que almacena los mensajes que entran 
        /// desde los clientes del API 
        /// ALTA PRIORIDAD
        /// </summary>
        private Stack<Message> clientInputStackHP;
        /// <summary>
        /// Pila que almacena los mensajes que deben ser
        /// enviados a los clientes del API
        /// ALTA PRIORIDAD
        /// </summary>
        private Stack<Message> clientOutPutStackHP;

        /// <summary>
        /// Pila que almacena los mensajes entrantes desde el servidor MQTT 
        /// ALTA PRIORIDAD
        /// </summary>
        private Stack<Message> serverInputPutStackHP;

        /// <summary>
        /// Pila que almacena los mensajes salientes hacia el servidor MQTT
        /// ALTA PRIORIDAD
        /// </summary>
        private Stack<Message> serverOutPutStackHP;

        /// <summary>
        ///  Recupera el estado general del broker de mensajes
        /// </summary>
        /// <returns></returns>
        public ActionResult<MesssageBrokerStatus> GetStatus()
        {
            lock (locker)
            {
                return BrokerStatusController.Instance.Status;
            }
        }

        /// <summary>
        ///  Vacía el estado general del broker de mensajes
        /// </summary>
        /// <returns></returns>
        public ActionResult CleanStatus()
        {
            lock (locker)
            {
                BrokerStatusController.Instance.CleanUp();
                return new OkResult();
            }
        }
        public ActionResult<RequestedOperationFailed> SendMessageToServer(string clientId, string json, EPriority priority = EPriority.Low)
        {
            switch (priority)
            {

                case EPriority.Any:
                    /*no se puede encolar un mensaje sin especificar la prioridad:
                     * este caso no se daría nunca dado a que la propia organización del recurso REST lo impide
                     * pero en todo caso no es redundante controlarlo explícitamente
                     */
                    return new RequestedOperationFailed()
                    {
                        Reason = "La prioridad no puede ser Any cuando se envía un mensaje"
                    };
                    break;

                case EPriority.High:
                    this.clientInputStackHP.Push(
                                                new Message
                                                {
                                                    ClientId = clientId,
                                                    JsonObject = json,
                                                    Priority = EPriority.High
                                                });
                    BrokerStatusController.Instance.NotifyQueuedMessage(BrokerStatusController.EDirection.FromClientToBroker);

                    try
                    {
                        ActionResult actionResult = MQTTController.Instance.SendMessage(this.clientInputStackHP.Peek());
                        if (actionResult is OkResult)
                        {
                            this.clientInputStackHP.Pop();
                            BrokerStatusController.Instance.NotifyProcessedMessage(BrokerStatusController.EDirection.FromClientToBroker);
                            BrokerStatusController.Instance.NotifyProcessedMessage(BrokerStatusController.EDirection.FromBrokerToServer);
                        }
                        else
                        {

                        }
                    }
                    catch (Exception e)
                    {
                        return new RequestedOperationFailed { Reason = e.Message, StackTrace = e.StackTrace };
                    }


                    break;

                case EPriority.Low:
                    this.clientInputStackLP.Push(
                                                 new Message
                                                 {
                                                     ClientId = clientId,
                                                     JsonObject = json,
                                                     Priority = EPriority.Low
                                                 });
                    BrokerStatusController.Instance.NotifyQueuedMessage(BrokerStatusController.EDirection.FromClientToBroker);
                    try
                    {
                        ActionResult actionResult = MQTTController.Instance.SendMessage(this.clientInputStackLP.Peek());
                        if (actionResult is OkResult)
                        {
                            this.clientInputStackHP.Pop();
                            BrokerStatusController.Instance.NotifyProcessedMessage(BrokerStatusController.EDirection.FromClientToBroker);
                            BrokerStatusController.Instance.NotifyProcessedMessage(BrokerStatusController.EDirection.FromBrokerToServer);
                        }
                        else
                        {

                        }
                    }
                    catch (Exception e)
                    {
                        return new RequestedOperationFailed { Reason = e.Message, StackTrace = e.StackTrace };
                    }


                    break;
            }


            return new OkResult();
        }

        /// <summary>
        /// Recupera el siguiente mensaje disponible o todos los mensajes si all=true
        /// </summary>
        /// <param name="all">retorna todos los mensajes si el valor es verdadero</param>
        /// <param name="priority"> si Alta retorna de la cola de alta prioridad
        /// Low de la cola de baja
        /// Any busca primero en la cola de alta, si no hay mensajes, recupera de la cola de baja.
        /// </param>
        /// <returns></returns>
        public ActionResult<List<Message>> GetMessagesFromServer(bool all = false, EPriority priority = EPriority.Any)
        {
            return new List<Message>();
        }
    }
}
