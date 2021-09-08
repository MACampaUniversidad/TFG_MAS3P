using System.Collections;
using System.Text;
using TFG_MAS3pProtocol.Definitions;

namespace TFG_MAS3pProtocol.Factories
{
    public static class PacketFactory
    {

        public static IMAS3pMessagePacket CreatePacket(EMAS3pDirection direction, EMAS3pMessageTypes type, EMAS3pActionTypes action, string packetValue)
        {
            //crear la cabecera  de un byte( 8 bits). Los dos primeros bits indican la dirección, dos siguientes el tipo, los 2 siguientes acción,los dos siguientes un espacio en blanco, :
            //[00 00 00 00]
            //se deja espacio de bits para ampliar el protocolo en el caso de ser necesario en el futuro
            //por ejemplo el primer par de bits 01 para nodo a nube, 10 para nube a nodo, 11 podría ser para un broadcast a todos los nodos y nubes, 00 broadcast a todos los nodos 
            //o similar.
            //
            //El último par de bits se deja a false como separador 
            BitArray header = new BitArray(8, false);
            switch (direction)
            {
                case EMAS3pDirection.NodeToCloudb:
                    header[0] = true;
                    header[1] = false;
                    break;
                case EMAS3pDirection.CloudToNode:
                    header[0] = false;
                    header[1] = true;
                    break;
            }
            switch (type)
            {
                case EMAS3pMessageTypes.Announce:
                    header[2] = false;
                    header[3] = true;
                    break;
                case EMAS3pMessageTypes.Configuration:
                    header[2] = true;
                    header[3] = false;
                    break;
                case EMAS3pMessageTypes.Measurement:
                    header[2] = true;
                    header[3] = true;
                    break;
            }
            switch (action)
            {
                case EMAS3pActionTypes.Request:
                    header[4] = false;
                    header[5] = true;
                    break;
                case EMAS3pActionTypes.Response:
                    header[4] = true;
                    header[5] = false;
                    break;
                case EMAS3pActionTypes.Update:
                    header[4] = true;
                    header[5] = true;
                    break;
            }
            //ultimo par de bits en baja.
            header[6] = false;
            header[7] = false;

            //mensaje

            byte[] valueBytes = ASCIIEncoding.ASCII.GetBytes(packetValue);
            byte[] message = new byte[valueBytes.Length + 1];
            //montar todo:
            header.CopyTo(message, 0);
            valueBytes.CopyTo(message, 1);

            return new MAS3pPacket(message);
        }

    }
}
