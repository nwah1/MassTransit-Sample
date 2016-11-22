using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Model;
using Demo.Receiver.Context;
using MassTransit;

namespace Demo.Receiver
{
    public class SaveDataConsumer : IConsumer<SaveData>
    {
        public Task Consume(ConsumeContext<SaveData> context)
        {
            SaveData dataToSave = context.Message;

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

            Console.WriteLine("Repos have been saved with the latest data. Details: ");
            Console.WriteLine("Count: " + dataToSave.Repos.Count);

            return Task.FromResult(context.Message);
        }
    }
}