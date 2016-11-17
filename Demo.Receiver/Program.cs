using System;
using Demo.Model;
using MassTransit;

namespace Demo.Receiver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                var rabbitMqHost = rabbit.Host(new Uri(Consts.RabbitMqAddress), settings =>
                {
                    settings.Username(Consts.User);
                    settings.Password(Consts.Pass);
                });

                rabbit.ReceiveEndpoint(rabbitMqHost, Consts.RabbitMqQueue, conf =>
                {
                    conf.Consumer<LoadDataConsumer>();
                });
            });

            rabbitBusControl.Start();
            Console.ReadKey();

            rabbitBusControl.Stop();
        }
    }
}
