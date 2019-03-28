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
            return new RabbitEventProperties
            {
                ContentType = p.ContentType,
                MessageId = p.MessageId,
                DeliveryMode = p.DeliveryMode,
                Headers = (Dictionary<string, object>) p.Headers,
            };
        }
    }
}
