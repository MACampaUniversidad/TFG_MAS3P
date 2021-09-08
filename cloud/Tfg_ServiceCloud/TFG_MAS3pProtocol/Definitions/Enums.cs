using System;

namespace TFG_MAS3pProtocol.Definitions
{
    /// <summary>
    /// Tipo de mensaje
    /// </summary>
    public enum EMAS3pMessageTypes 
    {
        /// <summary>
        /// Mensajes de configuración : nube - nodo 
        /// </summary>
        Configuration = 10,
        /// <summary>
        /// mensajes de medidas :nodo - nube
        /// </summary>
        Measurement = 15, 
        /// <summary>
        /// anuncios a la red. pueden ir en los dos sentidos, nodo - nube o nube - nodo 
        /// </summary>
        Announce=20
    }
    /// <summary>
    /// Dirección en la que va el mensaje 
    /// </summary>
    public enum EMAS3pDirection
    {
        NodeToCloudb = 1,
        CloudToNode = 2,
    }


    public enum EMAS3pActionTypes 
    {
        /// <summary>
        /// Petición de datos 
        /// </summary>
        Request = 10,
        /// <summary>
        /// Variación de la configuración del dispositivo 
        /// </summary>
        Update = 15,
        /// <summary>
        /// Mensajes de respuesta.
        /// </summary>
        Response = 20
    }


  
}
