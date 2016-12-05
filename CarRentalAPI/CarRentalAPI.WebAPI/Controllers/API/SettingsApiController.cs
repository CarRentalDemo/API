using CarRentalAPI.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;

namespace CarRentalAPI.WebAPI.Controllers.API
{
    [RoutePrefix("api/Settings")]
    public class SettingsApiController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<Setting> GetSettingsAsync()
        {
            using (var db = new CarRentalDatabase())
            {
                return await db.Settings.FirstOrDefaultAsync();
            }
        }

        [HttpPut]
        [Route("")]
        public async Task UpdateSettingsAsync([FromBody] Setting newSetting)
        {
            using (var db = new CarRentalDatabase())
            {
                var setting = await db.Settings.FirstOrDefaultAsync();

                setting.DayPrice = newSetting.DayPrice;
                setting.KilometerPrice = newSetting.KilometerPrice;

                await db.SaveChangesAsync();
            }
        }
    }
}
