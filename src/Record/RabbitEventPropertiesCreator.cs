using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Impl;
using RabbitReplay.Shared.Entities;

namespace RabbitReplay.Record
{
    public class RabbitEventPropertiesCreator
    {
        public static RabbitEventProperties Create(IBasicProperties p)
        {
            // RMQ, I dunno... properties and headers within headers and properties?
            if (!(p.Headers["properties"] is Dictionary<string, object> properties) || !(properties["headers"] is Dictionary<string, object> headers))
            {
                return new RabbitEventProperties();
            }

            // in RMQ headers are just a bundle of strings, so this should be safe.
            var decodedHeaders = headers.ToDictionary(
                d => d.Key,
                d => Encoding.UTF8.GetString((byte[])d.Value));

            return new RabbitEventProperties
            {
                ContentType = DecodeHelpers.StringFromByteDictionaryOrDefault(properties, "content_type"),
                MessageId = DecodeHelpers.StringFromByteDictionaryOrDefault(properties, "message_id"),
                DeliveryMode = (int)properties["delivery_mode"],
                Headers = decodedHeaders,
            };
        }
    }
}
