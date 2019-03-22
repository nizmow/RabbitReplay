using System;

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

        public string[] RoutedKeys { get; set; }

        public DateTime Timestamp { get; set; }

        public string Type { get; set; }

        public string User { get; set; }

        public string VHost { get; set; }
    }
}
