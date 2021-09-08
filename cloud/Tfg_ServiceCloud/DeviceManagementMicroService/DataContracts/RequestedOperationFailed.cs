using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceManagementMicroservice.DataContracts
{
    public class RequestedOperationFailed
    {
        /// <summary>
        /// Mensaje general de error
        /// </summary>
        string Reason { get; set; }
        /// <summary>
        /// Qué operación se estaba solicitando cuando se ha producido el fallo
        /// </summary>
        string OperationRequested { get; set; }
        /// <summary>
        /// Stack de error.
        /// </summary>
        string StackTrace { get; set; }
    }
}
