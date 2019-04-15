using System.Collections.Generic;

namespace RabbitReplay.Shared.Entities
{
    public class RabbitEventProperties
    {
        public string ContentType { get; set; }

        public int DeliveryMode { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string MessageId { get; set; }
    }
}
