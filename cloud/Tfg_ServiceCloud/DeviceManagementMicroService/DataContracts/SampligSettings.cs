using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceManagementMicroservice.DataContracts
{
    public class SampligSettings
    {
        /// <summary>
        /// Cuando la bateria tiene una carga inferior al valor el dispositivo se realimenta hasta alcanzarla.
        /// Una vez alcanzada se realizarán muestreos. Hasta ese momento permanecerá desactivado el sistema de muestreo.
        /// Valor está acotado entre 0  - 100
        /// </summary>
        public ushort MinimunOperationalBatteryChargePercent  {get;set;}
        /// <summary>
        /// Establece en minutos cada cuanto tiempo se va a realizar el muestreo. El valor recomendado es 1.
        /// </summary>
        public ushort SamplingPeriod { get; set; }
    }
}
