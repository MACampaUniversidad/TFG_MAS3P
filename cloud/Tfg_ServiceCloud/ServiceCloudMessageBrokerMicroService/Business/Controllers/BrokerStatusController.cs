using ServiceCloudMessageBrokerMicroService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudMessageBrokerMicroService.Business.Controllers
{
    /// <summary>
    /// Gestiona el estado general del sistema  -patron Singleton
    /// </summary>
    internal sealed class BrokerStatusController : IBrokerStatusController
    {
        /// <summary>
        /// Singleton en modo "lazy"
        /// </summary>
        private static readonly Lazy<BrokerStatusController> lazyInstance =
            new Lazy<BrokerStatusController>(() => new BrokerStatusController());

        public static BrokerStatusController Instance { get { return lazyInstance.Value; } }

        private BrokerStatusController()
        {
            this.Status = new MesssageBrokerStatus();
        }

        public enum EDirection
        {
            FromServerToBroker,
            FromBrokerToClient,
            FromClientToBroker,
            FromBrokerToServer,
        }
        public MesssageBrokerStatus Status { get; private set; }
        /// <summary>
        /// Notifica de la llegada a una de las colas de un mensaje incrementando la 
        /// estadística correspondiente a la cola de dirección especificada.
        /// </summary>
        /// <param name="direction"></param>
        public void NotifyQueuedMessage(EDirection direction)
        {
            if (this.Status.ToBeProcessed == null) this.Status.ToBeProcessed = new QueueMessagesStatistics();

            switch (direction)
            {
                case EDirection.FromBrokerToClient:
                    this.Status.ToBeProcessed.QueuedMessagesToClients++;
                    break;
                case EDirection.FromBrokerToServer:
                    this.Status.ToBeProcessed.QueuedMessagesToServer++;
                    break;
                case EDirection.FromClientToBroker:
                    this.Status.ToBeProcessed.QueuedMessagesFromClients++;
                    break;
                case EDirection.FromServerToBroker:
                    this.Status.ToBeProcessed.QueuedMessagesFromServer++;
                    break;
            }
        }
        /// <summary>
        /// Actualiza el estado general del broker. Automáticamente retira el valor en
        /// las estadísticas de colas de envíos manteniendo la homogeneidad entre lo procesado 
        /// y lo enviado
        /// </summary>
        /// <param name="direction"></param>
        public void NotifyProcessedMessage(EDirection direction)
        {
            //no se debería de llegar nunca con el ToBeProcessed a null, pero en cualquier caso.
            if (this.Status.ToBeProcessed == null) this.Status.ToBeProcessed = new QueueMessagesStatistics();
            if (this.Status.Processed == null) this.Status.Processed = new MessagesStatistics();

            switch (direction)
            {
                case EDirection.FromBrokerToClient:
                    this.Status.Processed.SentMessagesToClients++;
                    this.Status.ToBeProcessed.QueuedMessagesToClients--;
                    this.Status.Processed.LastSentMessage = DateTime.UtcNow;
                    break;
                case EDirection.FromBrokerToServer:
                    this.Status.Processed.SentMessagesToServer++;
                    this.Status.ToBeProcessed.QueuedMessagesToServer--;
                    this.Status.Processed.LastSentMessage = DateTime.UtcNow;
                    break;
                case EDirection.FromClientToBroker:
                    this.Status.Processed.ReceivedMessagesFromClients++;
                    this.Status.ToBeProcessed.QueuedMessagesFromClients--;
                    this.Status.Processed.LastReceivedMessage = DateTime.UtcNow;
                    break;
                case EDirection.FromServerToBroker:
                    this.Status.Processed.ReceivedMessagesFromServer++;
                    this.Status.ToBeProcessed.QueuedMessagesFromServer--;
                    this.Status.Processed.LastReceivedMessage = DateTime.UtcNow;
                    break;
            }
        }

        public void NotifyConnectionChange(bool active)
        {
            this.Status.IsConnected = active;
        }
        public void NotifyRequestFailure(RequestedOperationFailed e)
        {
            if (this.Status.LastOperatonFailures == null)
            {
                List<QueueMessagesStatistics> queueMessagesStatistics = new List<QueueMessagesStatistics>();
                this.Status.LastOperatonFailures = (IEnumerable<RequestedOperationFailed>)queueMessagesStatistics;
            }
           ((IList<RequestedOperationFailed>)this.Status.LastOperatonFailures).Add(e);

        }
        public void CleanUp()
        {
            this.Status = new MesssageBrokerStatus();
        }

    }
}
