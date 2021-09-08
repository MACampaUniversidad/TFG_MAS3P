using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_MAS3pProtocol.Definitions
{
    public sealed class MAS3pPacket : IMAS3pMessagePacket
    {
        private byte[] payload;
        internal MAS3pPacket(byte[] payload)
        {
            this.payload = payload;
        
        }
            
        public EMAS3pDirection GetDirection()
        {
            throw new NotImplementedException();
        }

        public EMAS3pActionTypes GetMessageAction()
        {
            throw new NotImplementedException();
        }

        public string GetMessagePayload()
        {
            return System.Convert.ToBase64String(payload); ;
        }

        public EMAS3pMessageTypes GetMessageType()
        {
            throw new NotImplementedException();
        }
    }
}
