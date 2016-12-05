using CarRentalAPI.WebAPI.Model;
using System.Threading.Tasks;
using System.Web.Http;

namespace CarRentalAPI.WebAPI.Controllers.API
{
    [RoutePrefix("api/Pricing")]
    public class PricingApiController : ApiController
    {
        [HttpPost]
        [Route("FinalPrice")]
        public async Task<decimal> GetFinalPrice([FromBody] FinalPriceModel model)
        {
            return await model.CalculateFinalPriceAsync();
        }
    }
}
