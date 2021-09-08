namespace ServiceCloudMessageBrokerMicroService.DataContracts
{
    /// <summary>
    /// Contiene estadísticas relativas a los mensajes que están pendientes de
    /// ser procesados y están en las colas del broker.
    /// </summary>
    public class QueueMessagesStatistics
    {
        /// <summary>
        /// En cola recibidos por el servidor para ser procesados.
        /// </summary>
        public ulong QueuedMessagesFromServer { get; set; }
        /// <summary>
        /// En cola recibidos desde los clientes
        /// </summary>
        public ulong QueuedMessagesFromClients { get; set; }
        /// <summary>
        /// Mensajes en cola de ser enviados al servidor
        /// </summary>
        public ulong QueuedMessagesToServer { get; set; }
        /// <summary>
        /// Mensajes en cola de ser enviados a los clientes
        /// </summary>
        public ulong QueuedMessagesToClients { get; set; }
    }
}