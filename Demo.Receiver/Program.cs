using System;
using Demo.Model;
using MassTransit;
using MassTransit.Util;

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

                rabbit.ReceiveEndpoint(rabbitMqHost, Consts.LoadDataQueue, conf =>
                {
                    conf.Consumer<SaveDataConsumer>();
                });

                rabbit.ReceiveEndpoint(rabbitMqHost, Consts.ListReposQueue, conf =>
                {
                    conf.Consumer<ListReposConsumer>();
                });
            });

            TaskUtil.Await(() => rabbitBusControl.StartAsync());            

            Console.ReadKey();

            rabbitBusControl.Stop();
        }
    }
}
