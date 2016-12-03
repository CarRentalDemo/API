using CarRentalAPI.Data;
using System.Threading.Tasks;
using System.Web.Http;

namespace CarRentalAPI.WebAPI.Controllers
{
    public class HealthController : ApiController
    {
        /// <summary>
        /// Checks current health of the API and related components
        /// </summary>
        /// <returns></returns>
        [Route("health")]
        [HttpGet]
        public async Task<string> GetHealthAsync()
        {
            using (var db = new CarRentalDatabase())
            {
                await db.Database.ExecuteSqlCommandAsync("select GETDATE()");
            }

            return "OK";
        }
    }
}
