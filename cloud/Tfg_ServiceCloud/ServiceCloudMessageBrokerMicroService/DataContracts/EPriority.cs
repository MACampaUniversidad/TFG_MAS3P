using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudMessageBrokerMicroService.DataContracts
{
    /// <summary>
    /// Define la prioridad de un mensaje.
    /// </summary>
    public enum EPriority
    {
        /// <summary>
        /// Cualquier priodidad, no debe de usarse para encolar mensajes 
        /// </summary>
        Any = 0,
        /// <summary>
        /// 
        /// </summary>
        Low = 1,
        /// <summary>
        /// Mensajes de configuración y de estado del sistema
        /// </summary>
        High = 2
    }
}
