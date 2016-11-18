using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Demo.Model;
using MassTransit;
using Newtonsoft.Json;

namespace Demo.Web.Controllers
{
    [AllowAnonymous]
    public class LoadDataController : ApiController
    {
        private IBusControl _busControl = null;

        // GET api/LoadData
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

            if (_busControl == null)
            {
                _busControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
                {
                    rabbit.Host(rabbitMqRootUri, settings =>
                    {
                        settings.Username(Consts.User);
                        settings.Password(Consts.Pass);
                    });
                });
            }

            await _busControl.Publish<ILoadData>(new
            {
                Id = Guid.NewGuid(),
                Repos = repos
            });
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
