using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CloudMessageBrokerMicroService.Business.MQTTMessageDefinitions
{
    /// <summary>
    /// Ver https://www.thethingsnetwork.org/docs/applications/mqtt/api/
    /// </summary>
    public class DownLinkMessage
    {
        [JsonPropertyName("port")]
        public int f_port { get; set; } = 1;
        [JsonPropertyName("confirmed")]
        public bool Confirmed { get; set; } = false;
        [JsonPropertyName("payload_fields")]
        public string payload_fields { get; set; }
        [JsonPropertyName("priority")]
        public string priority { get; set; } = "NORMAL";
    }
}
