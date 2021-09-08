using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceManagementMicroservice.DataContracts
{
#nullable enable
    /// <summary>
    /// Configuración completa de un dispositivo.
    /// </summary>
    public class DeviceSettings
    {

        /// <summary>
        /// permite desactivar el dispositivo
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Descripción de dónde se encuentra el dispositivo ubicado.
        /// </summary>
        public string? UbicationDescription { get; set; }
        /// <summary>
        /// Algunos dispositivos pueden no tener incorporado un GPS , este campo permite
        /// establecer manualmente la posición del dispositivo
        /// </summary>
        public GeoCoordinate?  Coordinates { get; set; }
        /// <summary>
        /// Establece la configuración de muestreo del dispositivo.
        /// </summary>
        public SampligSettings SampligSettings { get; set; }
    }
}
