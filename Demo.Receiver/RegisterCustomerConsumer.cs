using System;
using System.Threading.Tasks;
using MassTransit;
using Pangea.Messaging;

namespace Demo.Receiver
{
    public class RegisterCustomerConsumer : IConsumer<IRegisterCustomer>
    {
        public Task Consume(ConsumeContext<IRegisterCustomer> context)
        {
            IRegisterCustomer newCustomer = context.Message;
            Console.WriteLine("A new customer has signed up, it's time to register it. Details: ");
            Console.WriteLine(newCustomer.Address);
            Console.WriteLine(newCustomer.Name);
            Console.WriteLine(newCustomer.Id);
            Console.WriteLine(newCustomer.Preferred);
            return Task.FromResult(context.Message);
        }
    }
}