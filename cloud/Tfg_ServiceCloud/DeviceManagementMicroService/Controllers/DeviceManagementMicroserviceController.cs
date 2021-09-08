using DeviceManagementMicroservice.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tfg_ServiceCloud.Controllers
{
    /// <summary>
    /// Se ocupa de la configuración de los dispositivos en la red de sensores
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DeviceManagementMicroserviceController : ControllerBase
    {
      
        private readonly ILogger<DeviceManagementMicroserviceController> _logger;

        public DeviceManagementMicroserviceController(ILogger<DeviceManagementMicroserviceController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la configuración de un Dispositivo o de todos los dispositivos cuando el parámetro
        /// id no es informado.
        /// </summary>
        /// <param name="id">El identificador de dispositivo, nulo si se desea obtener todo el listado</param>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="404">Se ha procesado la operación sin errores pero no se han encontrado dispositivos</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpGet]
        [Route("/devices/{id?}")]
        [ProducesResponseType(typeof(IEnumerable<DeviceSettings>), 200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 400)]
        [ProducesResponseType(typeof(RequestedOperationFailed),500)]
        public async Task<ActionResult<IEnumerable<DeviceSettings>>> Find(string? id)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new DeviceSettings
            {
             
                Enabled=true,
                Coordinates=new GeoCoordinate { Altitude=200, Latitude=100, longitude=30},
                UbicationDescription="example",
                SampligSettings= new SampligSettings { MinimunOperationalBatteryChargePercent=20, SamplingPeriod=2}
                
            })
            .ToArray();

        }

        /// <summary>
        /// Modifica la Configuración para un dispositivo
        /// </summary>
        /// <param name="id">El identificador de dispositivo </param>
        /// <param name="settings"> Configuración. </param>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="400">Cuando no se ha indicado correctamente el DeviceId</response>
        /// <response code="404">Se ha procesado la operación sin errores pero no se han encontrado el dispositivo</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpPut]
        [Route("/devices/{id}/{settings}")]
        [ProducesResponseType( 200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 400)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 404)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public async Task<IActionResult> Update (string deviceId, DeviceSettings deviceSettings)
        {
            if ( string.IsNullOrEmpty(deviceId))
            {
                return (IActionResult)Task.FromResult(BadRequest());
            }
            return (IActionResult)Task.FromResult(Ok());
        }

        /// <summary>
        /// Modifica la configuración en minutos para cada cuanto tiempo se va a realizar el muestreo.
        /// </summary>
        /// <param name="deviceId">El identificador de dispositivo</param>
        /// <param name="samplingPeriod">Establece en minutos cada cuanto tiempo se va a realizar el muestreo. El valor recomendado es 1. </param>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="400">Cuando no se ha indicado correctamente el DeviceId</response>
        /// <response code="404">Se ha procesado la operación sin errores pero no se han encontrado el dispositivo</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpPatch]
        [Route("/devices/{deviceId}/sampling/{samplingPeriod}")]
        [ProducesResponseType( 200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 400)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 404)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public async Task<IActionResult> UpdateSampling(string deviceId, ushort samplingPeriod)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return (IActionResult)Task.FromResult(BadRequest());
            }
            return (IActionResult)Task.FromResult(Ok());
        }
        /// <summary>
        /// Modifica el valor mínimo de carga de batería para que el dispositivo realice muestreos.
        /// Una vez alcanzada se realizarán muestreos. Hasta ese momento permanecerá desactivado el sistema de muestreo
        /// el valor está acotado entre 0 - 100
        /// 
        /// </summary>
        /// <param name="deviceId">El identificador de dispositivo</param>
        /// <param name="minimunOperationalBatteryCharge"> Cuando la batería tiene una carga inferior al valor el dispositivo se re alimenta hasta alcanzarla.</param>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="400">Cuando no se ha indicado correctamente el DeviceId</response>
        /// <response code="404">Se ha procesado la operación sin errores pero no se han encontrado el dispositivo</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpPatch]
        [Route("/devices/{deviceId}/sampling/{minimunOperationalBatteryCharge}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 400)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 404)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public  IActionResult UpdateMinimunOperationalBatteryCharge(string deviceId, ushort minimunOperationalBatteryCharge)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return (IActionResult)Task.FromResult(BadRequest());
            }
            return (IActionResult)Task.FromResult(Ok());
        }

        /// <summary>
        /// Modifica la posición geográfica de un dispositivo : Latitud
        /// </summary>
        /// <param name="deviceId">El identificador de dispositivo</param>
        /// <param name="latitude">Nuevas coordenadas del dispositivo</param>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="404">Se ha procesado la operación sin errores pero no se han encontrado el dispositivo</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpPatch]
        [Route("/devices/{deviceId}/location/{latitude}")]
        [ProducesResponseType( 200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 404)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public  IActionResult UpdateGeoCordinatesLatitude(string deviceId, double latitude)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return (IActionResult)Task.FromResult(BadRequest());
            }
            return (IActionResult)Task.FromResult(Ok());
        }
        /// <summary>
        /// Modifica la posición geográfica de un dispositivo : Longitud
        /// </summary>
        /// <param name="deviceId">El identificador de dispositivo</param>
        /// <param name="longitude">Nuevas coordenadas del dispositivo</param>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="404">Se ha procesado la operación sin errores pero no se han encontrado el dispositivo</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpPatch]
        [Route("/devices/{deviceId}/location/{longitude}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 404)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public  IActionResult UpdateGeoCordinatesLongitude(string deviceId, double longitude)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return (IActionResult)Task.FromResult(BadRequest());
            }
            return (IActionResult)Task.FromResult(Ok());
        }
        /// <summary>
        /// Modifica la posición geográfica de un dispositivo : Altitud
        /// </summary>
        /// <param name="deviceId">El identificador de dispositivo</param>
        /// <param name="altitude">Nuevas coordenadas del dispositivo</param>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="404">Se ha procesado la operación sin errores pero no se han encontrado el dispositivo</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpPatch]
        [Route("/devices/{deviceId}/location/{altitude}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 404)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public  IActionResult UpdateGeoCordinatesAltitude(string deviceId, double altitude)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return (IActionResult)Task.FromResult(BadRequest());
            }
            return (IActionResult)Task.FromResult(Ok());
        }

        /// <summary>
        /// Modifica la posición del panel solar - rotación eje X
        /// valor en grados 0-180
        /// </summary>
        /// <param name="deviceId">El identificador de dispositivo</param>
        /// <param name="xAxis">Nuevas coordenadas del dispositivo</param>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="404">Se ha procesado la operación sin errores pero no se han encontrado el dispositivo</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpPatch]
        [Route("/devices/{deviceId}/solarPanelAxis/{xAxis}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 404)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public IActionResult UpdatesolarPanelAxisX(string deviceId, short xAxis)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return (IActionResult)Task.FromResult(BadRequest());
            }
            return (IActionResult)Task.FromResult(Ok());
        }
        /// <summary>
        /// Modifica la posición del panel solar - rotación eje Y 
        /// valor en grados 0-180
        /// </summary>
        /// <param name="deviceId">El identificador de dispositivo</param>
        /// <param name="yAxis">Nuevas coordenadas del dispositivo</param>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="404">Se ha procesado la operación sin errores pero no se han encontrado el dispositivo</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpPatch]
        [Route("/devices/{deviceId}/solarPanelAxis/{yAxis}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 404)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public IActionResult UpdatesolarPanelAxisY(string deviceId, short yAxis)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return (IActionResult)Task.FromResult(BadRequest());
            }
            return (IActionResult)Task.FromResult(Ok());
        }
    }
}
