using System;
using CommandLine;

namespace RabbitReplay.App.Options
{
    public class GlobalOptions
    {
        public GlobalOptions(Uri rabbitUri)
        {
            RabbitUri = rabbitUri;
        }

        [Option('r', "rabbituri", Required = true, HelpText = "Location of the RabbitMQ host, should include username and password.")]
        public Uri RabbitUri { get; }
    }
}
