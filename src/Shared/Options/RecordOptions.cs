using System;
using CommandLine;

namespace RabbitReplay.Shared.Options
{
    [Verb("record", HelpText = "Record events from the RabbitMQ firehose")]
    public class RecordOptions : GlobalOptions
    {
        public RecordOptions(string outputFile, string routingKey, Uri rabbitUri) : base(rabbitUri)
        {
            OutputFile = outputFile;
            RoutingKey = routingKey;
        }

        [Option('o', "output", Required = true, HelpText = "File in which to save trace results.")]
        public string OutputFile { get; }

        [Option('k', "key", Required = false, Default = "#", HelpText = "Routing key for recorded events.")]
        public string RoutingKey { get; }
    }
}
