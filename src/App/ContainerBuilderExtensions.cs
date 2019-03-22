using Autofac;
using Autofac.Builder;
using CommandLine;
using RabbitReplay.App.Options;
using RabbitReplay.Record;
using RabbitReplay.Replay;
using RabbitReplay.Shared;

namespace RabbitReplay.App
{
    public static class ContainerBuilderExtensions
    {
        public static void ProcessArguments(this ContainerBuilder builder, params string[] args)
        {
            var parsed = Parser.Default.ParseArguments<RecordOptions, ReplayOptions>(args) as Parsed<object>;

            if (parsed == null)
            {
                return;
            }

            builder.RegisterInstance(parsed.Value).AsSelf().As<GlobalOptions>();

            if (parsed.Value is RecordOptions)
            {
                builder.RegisterType<Recorder>().AsImplementedInterfaces();
            }
            else if (parsed.Value is ReplayOptions)
            {
                builder.RegisterType<Replayer>().AsImplementedInterfaces();
            }
        }
    }
}
