using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudMessageBrokerMicroService.DataContracts
{
    /// <summary>
    /// Mensaje 
    /// </summary>
    public sealed class Message
    {
        public string ClientId { get; set; }
        /// <summary>
        /// Define la prioridad de envio de este mensaje
        /// </summary>
        public EPriority Priority { get; set; }
        /// <summary>
        /// Mensaje en formato JSON 
        /// </summary>
        public string JsonObject { get; set; }
    }
}
