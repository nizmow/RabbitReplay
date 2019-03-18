using System;
using System.Threading.Tasks;
using Autofac;
using GreenPipes;
using MassTransit;

namespace MassTransitProducer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.AddMassTransit(cbc =>
            {
                cbc.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host("localhost", "/");
                }));
            });
            var container = builder.Build();

            var busControl = container.Resolve<IBusControl>();
            await busControl.StartAsync();

            Console.CancelKeyPress += (sender, eventArgs) => busControl.Stop();

            var publisher = container.Resolve<IPublishEndpoint>();
            while (true)
            {
                await publisher.Publish<TestMessage>(new
                {
                    Data = "This is some data!"
                });
            }
        }
    }
}
