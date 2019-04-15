using System.Collections.Generic;
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

            return new RabbitEventProperties
            {
                ContentType = DecodeHelpers.FromBase64DictionaryKeyOrDefault(properties, "content_type"),
                MessageId = DecodeHelpers.FromBase64DictionaryKeyOrDefault(properties, "message_id"),
                DeliveryMode = (int)properties["delivery_mode"],
                Headers = DecodeHelpers.ConvertBase64Dictionary(headers),
            };
        }
    }
}
