using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Demo.Model;
using Demo.Receiver.Context;
using MassTransit;

namespace Demo.Receiver
{
    public class ListReposConsumer : IConsumer<ListRepos>
    {
        private PropertyInfo[] _propertyInfos;

        public Task Consume(ConsumeContext<ListRepos> context)
        {
            Console.WriteLine("The following repositories are in the database: ");
            Console.WriteLine();
            
            using (var repoContext = new RepoContext())
            {
                foreach (var repo in repoContext.Repos)
                {
                    var row = RepoToString(repo);

                    Console.WriteLine(row);
                }
            }

            return Task.FromResult(context.Message);
        }


        public StringBuilder RepoToString(Repo repo)
        {
            if (_propertyInfos == null)
                _propertyInfos = repo.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _propertyInfos)
            {
                var value = info.GetValue(repo, null) ?? "(null)";
                sb.Append(info.Name);
                sb.Append(": ");
                sb.Append(value);
                sb.Append(' ');
                sb.AppendLine();
            }

            return sb;
        }
    }
}