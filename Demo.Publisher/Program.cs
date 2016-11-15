using System;
using MassTransit;
using Pangea.Messaging;

namespace Demo.Publisher
{
    internal class Program
    {
        public static object RegisterNewOrderConsumer { get; private set; }

        private static void Main(string[] args)
        {
            RunMassTransitPublisherWithRabbit();
        }

        private static void RunMassTransitPublisherWithRabbit()
        {
            var rabbitMqRootUri = new Uri(Consts.RabbitMqAddress);

            var rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                rabbit.Host(rabbitMqRootUri, settings =>
                {
                    settings.Username(Consts.User);
                    settings.Password(Consts.Pass);
                });
            });

            var uri = new Uri(string.Concat(Consts.RabbitMqAddress, "/", Consts.RabbitMqQueue));
            var sendEndpointTask = rabbitBusControl.GetSendEndpoint(uri);
            var sendEndpoint = sendEndpointTask.Result;

            var sendTask = sendEndpoint.Send<IRegisterCustomer>(new
            {
                Address = "New Street",
                Id = Guid.NewGuid(),
                Preferred = true,
                RegisteredUtc = DateTime.UtcNow,
                Name = "Nice people LTD",
                Type = 1,
                DefaultDiscount = 0
            });

            Console.ReadKey();
        }
    }
}