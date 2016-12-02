using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Demo.Model;
using Newtonsoft.Json;

namespace Demo.Web.Controllers
{
    [AllowAnonymous]
    public class LoadDataController : ApiController
    {
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
            
            await WebApiApplication.BusControl.Publish<ILoadData>(new
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
