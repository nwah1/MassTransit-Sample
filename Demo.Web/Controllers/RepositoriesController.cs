using System.Threading.Tasks;
using System.Web.Http;
using Demo.Model;

namespace Demo.Web.Controllers
{
    [AllowAnonymous]
    public class RepositoriesController : ApiController
    {
        // GET api/LoadData
        public async Task Get()
        {
            await WebApiApplication.BusControl.Publish(new ListRepos());
        }
    }
}
