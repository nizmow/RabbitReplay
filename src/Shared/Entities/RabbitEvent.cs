using System;
using Newtonsoft.Json;
using RabbitReplay.Shared.Serialization;

namespace RabbitReplay.Shared.Entities
{
    public class RabbitEvent
    {
        public int Channel { get; set; }

        public string Connection { get; set; }

        public string Exchange { get; set; }

        public string Node { get; set; }

        public string Payload { get; set; }

        public RabbitEventProperties Properties { get; set; }

        public string Queue { get; set; }

        [JsonProperty("routed_queues")]
        public string[] RoutedQueues { get; set; }

        [JsonProperty("routing_keys")]
        public string[] RoutingKeys { get; set; }

        /// <summary>
        /// The trace files from the RMQ plugin use a strange date format, so we'll try and stay compatible with it.
        /// </summary>
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime Timestamp { get; set; }

        public string Type { get; set; }

        public string User { get; set; }

        /// <summary>
        /// We don't camel case this to match the format.
        /// </summary>
        [JsonProperty("vhost")]
        public string VHost { get; set; }
    }
}
