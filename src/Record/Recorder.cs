using System;
using System.IO;
using System.IO.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitReplay.App.Options;

namespace RabbitReplay.Record
{
    public class Recorder : IDisposable
    {
        private const string FirehoseExchange = "amq.rabbitmq.trace";
        private const string RecorderQueue = "rabbitreplay_recorder";

        private readonly RecordOptions _options;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly Stream _outputStream;

        public Recorder(RecordOptions options, IFileSystem fileSystem)
        {
            _options = options;
            var factory = new ConnectionFactory
            {
                Uri = options.RabbitUri,
                AutomaticRecoveryEnabled = false
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _outputStream = fileSystem.File.Open("test.trace", FileMode.OpenOrCreate);
        }

        public int Record()
        {
            // create channel: not durable, exclusive, auto-delete
            _channel.QueueDeclare(RecorderQueue, false, true, true, null);
            _channel.QueueBind(RecorderQueue, FirehoseExchange, "#", null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, eventArgs) =>
            {
                Console.WriteLine(eventArgs.Body);
                _outputStream.Write(eventArgs.Body, 0, eventArgs.Body.Length);
            };

            _channel.BasicConsume(consumer, RecorderQueue, true);

            return 0;
        }

        public delegate Recorder Factory(RecordOptions options);

        public void Dispose()
        {
            Console.WriteLine("Disposing!");
            _outputStream?.Dispose();
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
