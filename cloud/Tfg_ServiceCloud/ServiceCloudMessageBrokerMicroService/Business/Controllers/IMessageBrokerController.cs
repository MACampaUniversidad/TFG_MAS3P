using CloudMessageBrokerMicroService.DataContracts;
using Microsoft.AspNetCore.Mvc;
using ServiceCloudMessageBrokerMicroService.DataContracts;
using System.Collections.Generic;

namespace CloudMessageBrokerMicroService.Business.Controllers
{
    public interface IMessageBrokerController
    {
        ActionResult CleanStatus();
        ActionResult<List<Message>> GetMessagesFromServer(bool all = false, EPriority priority = EPriority.Any);
        ActionResult<MesssageBrokerStatus> GetStatus();
        ActionResult<RequestedOperationFailed> SendMessageToServer(string clientId, string json, EPriority priority = EPriority.Low);
    }
}