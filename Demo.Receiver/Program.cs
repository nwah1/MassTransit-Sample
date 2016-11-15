using System;
using MassTransit;
using Pangea.Messaging;

namespace Demo.Receiver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            RunMassTransitReceiverWithRabbit();
        }

        private static void RunMassTransitReceiverWithRabbit()
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
                    conf.Consumer<RegisterCustomerConsumer>();
                });
            });

            rabbitBusControl.Start();
            Console.ReadKey();

            rabbitBusControl.Stop();
        }
    }
}
