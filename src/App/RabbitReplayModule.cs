using System.IO.Abstractions;
using Autofac;

namespace RabbitReplay.App
{
    public class RabbitReplayModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileSystem>().AsImplementedInterfaces();
        }
    }
}
