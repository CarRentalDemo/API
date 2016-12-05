using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CarRentalAPI.Data;
using System.Web.OData;

namespace CarRentalAPI.WebAPI.Controllers.OData
{
    public class SettingsController : ODataController
    {
        private CarRentalDatabase db = new CarRentalDatabase();
        
        // GET: odata/Settings
        public SingleResult<Setting> GetSettings()
        {
            return SingleResult.Create(db.Settings.Take(1));
        }

        // PUT: odata/Settings(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Setting> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Setting setting = await db.Settings.FindAsync(key);
            if (setting == null)
            {
                return NotFound();
            }

            patch.Put(setting);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(setting);
        }

        // PATCH: odata/Settings(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Setting> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Setting setting = await db.Settings.FindAsync(key);
            if (setting == null)
            {
                return NotFound();
            }

            patch.Patch(setting);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(setting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SettingExists(int key)
        {
            return db.Settings.Count(e => e.Id == key) > 0;
        }
    }
}
