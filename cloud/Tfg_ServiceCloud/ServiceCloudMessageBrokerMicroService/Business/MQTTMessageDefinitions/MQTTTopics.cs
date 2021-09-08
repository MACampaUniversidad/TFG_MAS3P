
namespace CloudMessageBrokerMicroService.Business.MQTTMessageDefinitions
{
    /// <summary>
    /// Constantes empleadas para la determinación de los "topic" o colas
    /// dentro de MQTT. Más detalles en
    /// https://www.thethingsindustries.com/docs/integrations/mqtt/
    /// </summary>
    public static class MQTTTopics
    {

        /// <summary>
        /// Mensajes entrantes desde el broker MQTT de TTN
        /// </summary>
        public const string TOPIC_UP = "up";
        /// <summary>
        /// Mensajes salientes que han sido encolados en el broker de TTN
        /// pero aún no han sido entregados
        /// </summary>
        public const string TOPIC_DOWN_QUEUED = "queued";
        /// <summary>
        /// Mensajes salientes que han sido enviados al dispositivo final
        /// </summary>
        public const string TOPIC_DOWN_SENT = "sent";
        /// <summary>
        /// Mensajes salientes que han fallado al ser enviados al dispositivo final
        /// </summary>
        public const string TOPIC_DOWN_FAILED = "failed";
        /// <summary>
        /// Mensajes de ACK entrantes después de una operación down-link
        /// </summary>
        public  const string TOPIC_DOWN_ACK = "ack";
        /// <summary>
        /// Mensajes de ACK entrantes después de una operación down-link
        /// </summary>
        public const string TOPIC_DOWN_NACK = "nack";
        /// <summary>
        /// Mensaje de entrada de un dispositivo nuevo en la red WSN
        /// </summary>
        public const string TOPIC_JOIN = "join";
        /// <summary>
        /// Wild-card que representa a todos los topic
        /// </summary>
        public const string TOPIC_ALL = "#";

    }
}
