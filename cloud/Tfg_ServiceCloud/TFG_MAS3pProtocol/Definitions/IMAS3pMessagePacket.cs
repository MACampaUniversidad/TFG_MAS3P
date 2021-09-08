using TFG_MAS3pProtocol.Definitions;

namespace TFG_MAS3pProtocol.Definitions
{
    public interface IMAS3pMessagePacket
    {
        public EMAS3pMessageTypes GetMessageType();
        public EMAS3pActionTypes GetMessageAction();
        public EMAS3pDirection GetDirection();
        public string GetMessagePayload();

    }
}