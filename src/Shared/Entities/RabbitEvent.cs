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

        public string[] RoutedQueues { get; set; }

        public string[] RoutingKeys { get; set; }

        /// <summary>
        /// The trace files from the RMQ plugin use a strange date format, so we'll try and stay compatible with it.
        /// </summary>
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime Timestamp { get; set; }

        public string Type { get; set; }

        public string User { get; set; }

        /// <summary>
        /// Tricksy property names.
        /// </summary>
        [JsonProperty("vhost")]
        public string VHost { get; set; }
    }
}
