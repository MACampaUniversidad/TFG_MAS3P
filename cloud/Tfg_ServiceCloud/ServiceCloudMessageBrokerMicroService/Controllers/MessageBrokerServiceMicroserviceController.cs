using CloudMessageBrokerMicroService.Business.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceCloudMessageBrokerMicroService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudMessageBrokerMicroService.Controllers
{
    /// <summary>
    /// Gestiona el envío y recepción de mensajes entre el API y la red de sensores 
    /// desplegada en el campo.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MessageBrokerServiceMicroserviceController : ControllerBase
    {
        private readonly ILogger<MessageBrokerServiceMicroserviceController> _logger;

        public MessageBrokerServiceMicroserviceController(ILogger<MessageBrokerServiceMicroserviceController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Recupera el siguiente mensaje disponible sin entregar en el broker
        /// </summary>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpGet]
        [Route("/messageBroker/next")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public async Task<ActionResult<IEnumerable<string>>> Next()
        {
               var rng = new Random();
         
         return Enumerable.Range(1, 5).Select(index => $@"Json_Serialization_{rng.Next()}")
            .ToArray();

        }

        /// <summary>
        /// Recupera todos los mensajes en cola desde el servidor  
        /// </summary>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpGet]
        [Route("/messageBroker/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public  ActionResult<IEnumerable<string>> All()
        {
            var rng = new Random();
             return Enumerable.Range(1, 5).Select(index => $@"Json_Serialization_{rng.Next()}")
            .ToArray();

        }
        /// <summary>
        /// Envía un mensaje JSON a la red de sensores con alta prioridad
        /// </summary>
        /// <param name="clientId">el identificador del Nodo</param>
        /// <param name="message">mensaje en formato JSON</param>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpPost]
        [Route("/messageBroker/{clientId}/highPriority/{message}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public ActionResult<RequestedOperationFailed> PostHp( string clientId, string message)
        {
            return MessageBrokerController.Instance.SendMessageToServer(clientId, message,  DataContracts.EPriority.High);
        }
        /// <summary>
        /// Envía un mensaje JSON a la red de sensores con baja prioridad
        /// </summary>
        /// <param name="clientId">El identificador del Nodo</param>
        /// <param name="message">mensaje en formato JSON</param>
        /// <response code="200">Se ha procesado la operación sin errores.</response>
        /// <response code="500">La operación ha fallado debido a un error interno</response>
        [HttpPost]
        [Route("/messageBroker/{clientId}/lowPriority/{message}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RequestedOperationFailed), 500)]
        public ActionResult<RequestedOperationFailed> PostLP( string clientId,  string message)
        {
            return MessageBrokerController.Instance.SendMessageToServer(clientId, message);
            
        }

    }
}
