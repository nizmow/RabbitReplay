using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitReplay.Shared;
using RabbitReplay.Shared.Options;

namespace RabbitReplay.Record
{
    public class Recorder : IDisposable, IProgramVerb
    {
        private const string FirehoseExchange = "amq.rabbitmq.trace";
        private const string RecorderQueue = "rabbitreplay_recorder";

        private readonly RecordOptions _options;
        private readonly string _routingKey;
        private readonly string _file;

        // IDisposable, remember to clean up!
        private readonly StreamWriter _textWriter;
        private readonly JsonTextWriter _jsonWriter;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Recorder(RecordOptions options, IFileSystem fileSystem)
        {
            _options = options;
            var factory = new ConnectionFactory
            {
                Uri = options.RabbitUri,
                AutomaticRecoveryEnabled = false
            };
            _file = options.OutputFile;
            _routingKey = options.RoutingKey;

            // disposables! clean them up!
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _textWriter = new StreamWriter(fileSystem.File.Open(_file, FileMode.OpenOrCreate));
            _jsonWriter = new JsonTextWriter(_textWriter);
        }

        public Task<int> Run(CancellationToken cancellationToken)
        {
            // create channel: not durable, exclusive, auto-delete
            _channel.QueueDeclare(RecorderQueue, false, true, true, null);
            _channel.QueueBind(RecorderQueue, FirehoseExchange, _routingKey, null);

            var consumer = new EventingBasicConsumer(_channel);

            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.None;
            serializer.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy
                {
                    // when we drop a dictionary into here (eg: headers) it makes sense to leave the keys AS IS.
                    ProcessDictionaryKeys = false,
                },
            };

            consumer.Received += (ch, eventArgs) =>
            {
                try
                {
                    var payload = RabbitEventCreator.Create(eventArgs);
                    serializer.Serialize(_jsonWriter, payload);
                    _textWriter.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            };

            _channel.BasicConsume(consumer, RecorderQueue, true);
            Console.WriteLine($"Recording from the firehose to '{_file}' with routing key '{_routingKey}'.");
            cancellationToken.WaitHandle.WaitOne();

            return Task.FromResult(0);
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing!");
            _channel?.Dispose();
            _connection?.Dispose();
            // I've no idea why .Dispose is protected here?
            ((IDisposable)_jsonWriter)?.Dispose();
            _textWriter?.Dispose();
        }
    }
}
