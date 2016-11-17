using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Model;
using Demo.Receiver.Context;
using MassTransit;

namespace Demo.Receiver
{
    public class LoadDataConsumer : IConsumer<ILoadData>
    {
        public Task Consume(ConsumeContext<ILoadData> context)
        {
            ILoadData newCustomer = context.Message;

            using (var repoContext = new RepoContext())
            {
                foreach (var repo in context.Message.Repos)
                {
                    var original = repoContext.Repos.FirstOrDefault(r => r.full_name == repo.full_name);

                    if (original == null)
                        repoContext.Repos.Add(repo);
                    else
                    {
                        repo.id = original.id;
                        repoContext.Entry(original).CurrentValues.SetValues(repo);
                    }
                }

                repoContext.SaveChanges();
            }

            Console.WriteLine("Repos have been updated with the latest data. Details: ");
            Console.WriteLine("ID: " + newCustomer.Id);
            Console.WriteLine("Count: " + newCustomer.Repos.Count);

            return Task.FromResult(context.Message);
        }
    }
}