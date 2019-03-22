using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CommandLine;
using RabbitReplay.App.Options;
using RabbitReplay.Shared;

namespace RabbitReplay.App
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // We bind our options later, but for now fast-fail. It's a bit of repetition.
            var results = Parser.Default.ParseArguments<RecordOptions, ReplayOptions>(args);
            if (results is NotParsed<object>)
            {
                return 1;
            }

            var builder = new ContainerBuilder();
            builder.ProcessArguments(args);
            builder.RegisterModule<RabbitReplayModule>();
            var container = builder.Build();

            // Handle ctrl-c properly so things get disposed.
            var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cancellationTokenSource.Cancel();
            };

            using (var scope = container.BeginLifetimeScope())
            {
                var program = scope.Resolve<IProgramVerb>();
                return await program.Run(cancellationTokenSource.Token);
            }
        }
    }
}
