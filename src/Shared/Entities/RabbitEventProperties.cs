using System.Collections.Generic;
using Newtonsoft.Json;

namespace RabbitReplay.Shared.Entities
{
    public class RabbitEventProperties
    {
        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("delivery_mode")]
        public int DeliveryMode { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}
