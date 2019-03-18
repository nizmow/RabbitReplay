using System;
using CommandLine;

namespace RabbitReplay.App.Options
{
    [Verb("record", HelpText = "Record events from the RabbitMQ firehose")]
    public class RecordOptions : GlobalOptions
    {
        public RecordOptions(string filename, Uri rabbitUri) : base(rabbitUri)
        {
            Filename = filename;
        }

        [Option('f', "file", Required = true, HelpText = "File in which to save trace results.")]
        public string Filename { get; }
    }
}
