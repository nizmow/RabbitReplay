using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitReplay.App.Options;
using RabbitReplay.Shared;

namespace RabbitReplay.Record
{
    public class Recorder : IDisposable, IProgramVerb
    {
        private const string FirehoseExchange = "amq.rabbitmq.trace";
        private const string RecorderQueue = "rabbitreplay_recorder";

        private readonly RecordOptions _options;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly Stream _outputStream;
        private readonly string _routingKey;
        private readonly string _file;
        private readonly StreamWriter _textWriter;
        private readonly JsonTextWriter _jsonWriter;
        private readonly IFileSystem _fileSystem;

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
            _file = options.OutputFile;
            _fileSystem = fileSystem;
            _routingKey = options.RoutingKey;
        }

        public Task<int> Run(CancellationToken cancellationToken)
        {
            // create channel: not durable, exclusive, auto-delete
            _channel.QueueDeclare(RecorderQueue, false, true, true, null);
            _channel.QueueBind(RecorderQueue, FirehoseExchange, _routingKey, null);

            var consumer = new EventingBasicConsumer(_channel);

            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.None;
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

            using (var textWriter = new StreamWriter(_fileSystem.File.Open(_file, FileMode.OpenOrCreate)))
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                consumer.Received += (ch, eventArgs) =>
                {
                    try
                    {
                        var payload = RabbitEventCreator.Create(eventArgs);
                        serializer.Serialize(jsonWriter, payload);
                        textWriter.WriteLine();
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
            }

            return Task.FromResult(0);
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing!");
            // I've no idea why .Dispose is protected here?
            ((IDisposable)_jsonWriter)?.Dispose();
            _textWriter?.Dispose();
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
