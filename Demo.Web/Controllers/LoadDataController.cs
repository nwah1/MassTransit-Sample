using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MassTransit;
using Newtonsoft.Json;
using Pangea.Messaging;

namespace Demo.Web.Controllers
{
    [AllowAnonymous]
    public class LoadDataController : ApiController
    {
        // GET api/values
        public async Task Get()
        {
            var json = GetJson(Consts.Source);

            var repos = JsonConvert.DeserializeObject<List<Repo>>(json);

            if (repos == null)
            {
                var message = new HttpResponseMessage { ReasonPhrase = "Could not deserialize JSON", StatusCode = HttpStatusCode.InternalServerError };
                throw new HttpResponseException(message);
            }

            var rabbitMqRootUri = new Uri(Consts.RabbitMqAddress);

            var control = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                rabbit.Host(rabbitMqRootUri, settings =>
                {
                    settings.Username(Consts.User);
                    settings.Password(Consts.Pass);
                });
            });

            await control.Publish<ILoadData>(new
            {
                Id = Guid.NewGuid(),
                Repos = repos
            });

            Console.ReadKey();
        }


        private static string GetJson(string uri)
        {
            using (var client = new WebClient())
            {
                const string userAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2;)";
                client.Headers.Add("user-agent", userAgent);
                return client.DownloadString(uri);
            }
        }
    }
}
