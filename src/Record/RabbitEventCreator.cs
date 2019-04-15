using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitReplay.Shared.Entities;

namespace RabbitReplay.Record
{
    public static class RabbitEventCreator
    {
        private const string UnsupportedString = "<unsupported>";

        public static RabbitEvent Create(BasicDeliverEventArgs e)
        {
            var properties = e.BasicProperties;
            var headers = properties.Headers;

            return new RabbitEvent
            {
                Channel = (int)headers["channel"],
                Connection = DecodeHelpers.StringFromByteDictionaryOrDefault(headers, "connection"),
                Exchange = DecodeHelpers.StringFromByteDictionaryOrDefault(headers, "exchange_name"),
                Node = DecodeHelpers.StringFromByteDictionaryOrDefault(headers, "node"),
                Payload = Convert.ToBase64String(e.Body),
                Properties = RabbitEventPropertiesCreator.Create(e.BasicProperties),
                User = DecodeHelpers.StringFromByteDictionaryOrDefault(headers, "user"),
                VHost = DecodeHelpers.StringFromByteDictionaryOrDefault(headers, "vhost"),
                Type = "published",
                Timestamp = DateTime.Now,

                // todo: the following is for queue capture, not yet supported
                Queue = UnsupportedString,
                RoutingKeys = new[] { UnsupportedString},
                RoutedQueues = new[] { UnsupportedString },
            };
        }

        private static string FromBase64Bytes(byte[] encodedBytes)
        {
            return string.Empty;
        }
    }
}
