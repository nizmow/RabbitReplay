using Autofac;
using CommandLine;
using RabbitReplay.App.Options;
using RabbitReplay.Record;
using RabbitReplay.Replay;

namespace RabbitReplay.App
{
    class Program
    {
        static int Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Recorder>().AsSelf();
            builder.RegisterModule<RabbitReplayModule>();
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                return Parser.Default.ParseArguments<RecordOptions, ReplayOptions>(args)
                    .MapResult(
                        (RecordOptions options) => container.Resolve<Recorder.Factory>().Invoke(options).Record(),
                        (ReplayOptions options) => container.Resolve<Replayer>().Replay(),
                        errors => 1);
            }
        }
    }
}
