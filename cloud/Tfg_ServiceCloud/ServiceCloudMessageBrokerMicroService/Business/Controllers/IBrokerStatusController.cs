using ServiceCloudMessageBrokerMicroService.DataContracts;

namespace CloudMessageBrokerMicroService.Business.Controllers
{
    internal interface IBrokerStatusController
    {
        MesssageBrokerStatus Status { get; }

        void CleanUp();
        void NotifyConnectionChange(bool active);
        void NotifyProcessedMessage(BrokerStatusController.EDirection direction);
        void NotifyQueuedMessage(BrokerStatusController.EDirection direction);
        void NotifyRequestFailure(RequestedOperationFailed e);
    }
}