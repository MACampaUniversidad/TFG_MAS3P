using System.Collections.Generic;

namespace ServiceCloudMessageBrokerMicroService.DataContracts
{
    /// <summary>
    /// Contiene el estado general del gestor de mensajes
    /// </summary>
    public class MesssageBrokerStatus
    {
        /// <summary>
        /// Client Id empleada para conectar al servidor MQTT
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// URI del servidor MQTT
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// Si es verdadero el broker esta conectado al servidor MQTT
        /// </summary>
        public bool IsConnected { get; set; }
        /// <summary>
        /// Detalle de los mensajes que se han procesado
        /// </summary>
        public MessagesStatistics Processed { get; set; }  
        /// <summary>
        /// Detalle de los mensajes que están en cola pero no se han procesado aún
        /// </summary>
        public QueueMessagesStatistics ToBeProcessed { get; set; } 
        /// <summary>
        /// Listado con los errores que se han producido en la aplicación
        /// </summary>
        public IEnumerable<RequestedOperationFailed> LastOperatonFailures { get; set; }

    }
}
