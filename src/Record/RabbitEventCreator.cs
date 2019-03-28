using RabbitMQ.Client.Events;
using RabbitReplay.Shared.Entities;

namespace RabbitReplay.Record
{
    public static class RabbitEventCreator
    {
        private const string UnsupportedString = "<unsupported>";

        public static RabbitEvent Create(BasicDeliverEventArgs e)
        {
            return new RabbitEvent
            {
                Channel = -1,
                Connection = UnsupportedString,
                Exchange = e.Exchange,
                Node = UnsupportedString,
                Payload = System.Convert.ToBase64String(e.Body),
                Properties = RabbitEventPropertiesCreator.Create(e.BasicProperties),
                Queue = UnsupportedString,
                RoutedKeys = new[] { e.RoutingKey },
                RoutedQueues = new[] { UnsupportedString },
            };
        }
    }
}
