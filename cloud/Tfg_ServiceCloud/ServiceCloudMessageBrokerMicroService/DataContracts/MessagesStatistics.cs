using System;

namespace ServiceCloudMessageBrokerMicroService.DataContracts
{
    /// <summary>
    /// Contiene estadísticas relativas al tráfico de mensajes entre  
    /// servidor, broker y clientes
    /// </summary>
    public class MessagesStatistics
    {
 
        /// <summary>
        /// último mensaje recibido -hacia clientes o hacia servidor 
        /// </summary>
        public DateTime LastReceivedMessage { get; set; }
        /// <summary>
        /// último mensaje enviado -hacia clientes o hacia servidor 
        /// </summary>
        public DateTime LastSentMessage { get; set; }
        /// <summary>
        /// Número total de mensajes recibidos desde el servidor
        /// </summary>
        public ulong ReceivedMessagesFromServer { get; set; }
        /// <summary>
        /// Número total de mensajes enviados al servidor
        /// </summary>
        public ulong SentMessagesToServer { get; set; }
        /// <summary>
        /// Número total de mensajes recibidos desde clientes
        /// </summary>
        public ulong ReceivedMessagesFromClients { get; set; }
        /// <summary>
        /// Número total de mensajes enviados a los clientes
        /// </summary>
        public ulong SentMessagesToClients { get; set; }
    }
}