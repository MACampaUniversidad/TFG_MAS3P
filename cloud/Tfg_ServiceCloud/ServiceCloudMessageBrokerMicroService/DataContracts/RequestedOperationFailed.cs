using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ServiceCloudMessageBrokerMicroService.DataContracts
{
    public class RequestedOperationFailed 
    {
    
        public RequestedOperationFailed([CallerMemberName] string operationRequested = "")
        {
            this.OperationRequested = operationRequested;
        }
        /// <summary>
        /// Mensaje general de error
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// Qué operación se estaba solicitando cuando se ha producido el fallo
        /// </summary>
       public  string OperationRequested { get; set; }
        /// <summary>
        /// Stack de error.
        /// </summary>
        public string StackTrace { get; set; }
    }
}
