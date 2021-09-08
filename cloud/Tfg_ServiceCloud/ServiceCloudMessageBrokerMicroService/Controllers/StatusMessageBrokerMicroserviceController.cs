using CloudMessageBrokerMicroService.Business.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceCloudMessageBrokerMicroService.DataContracts;


namespace Tfg_ServiceCloud.Controllers
{
    /// <summary>
    /// Estado general del broker de mensajes del API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StatusMessageBrokerMicroserviceController : ControllerBase
    {
        private readonly ILogger<StatusMessageBrokerMicroserviceController> _logger;

        public StatusMessageBrokerMicroserviceController(ILogger<StatusMessageBrokerMicroserviceController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Obtiene la información del estado general del broker de mensajes
        /// </summary>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpGet]
        [Route("/status/")]
        [ProducesResponseType(typeof(MesssageBrokerStatus), 200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public  ActionResult<MesssageBrokerStatus> All()
        {
            return  MessageBrokerController.Instance.GetStatus();
        }
        /// <summary>
        /// Reinicia toda la información de estado del broker
        /// </summary>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpDelete]
        [Route("/status/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public  ActionResult Delete()
        {
            return  MessageBrokerController.Instance.CleanStatus();
        }
        
    }
}

