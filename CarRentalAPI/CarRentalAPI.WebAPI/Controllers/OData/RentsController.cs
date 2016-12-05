using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using CarRentalAPI.Data;
using System.Web.OData;

namespace CarRentalAPI.WebAPI.Controllers.OData
{
    public class RentsController : ODataController
    {
        private CarRentalDatabase db = new CarRentalDatabase();

        // GET: odata/Rents
        [EnableQuery]
        public IQueryable<Rent> GetRents()
        {
            return db.Rents;
        }

        // GET: odata/Rents(5)
        [EnableQuery]
        public SingleResult<Rent> GetRent([FromODataUri] int key)
        {
            return SingleResult.Create(db.Rents.Where(rent => rent.Id == key));
        }

        // PUT: odata/Rents(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Rent> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Rent rent = await db.Rents.FindAsync(key);
            if (rent == null)
            {
                return NotFound();
            }

            patch.Put(rent);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rent);
        }

        // POST: odata/Rents
        public async Task<IHttpActionResult> Post(Rent rent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rents.Add(rent);
            await db.SaveChangesAsync();

            return Created(rent);
        }

        // PATCH: odata/Rents(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Rent> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Rent rent = await db.Rents.FindAsync(key);
            if (rent == null)
            {
                return NotFound();
            }

            patch.Patch(rent);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rent);
        }

        // DELETE: odata/Rents(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Rent rent = await db.Rents.FindAsync(key);
            if (rent == null)
            {
                return NotFound();
            }

            db.Rents.Remove(rent);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Rents(5)/Booking
        [EnableQuery]
        public SingleResult<Booking> GetBooking([FromODataUri] int key)
        {
            return SingleResult.Create(db.Rents.Where(m => m.Id == key).Select(m => m.Booking));
        }

        // GET: odata/Rents(5)/Client
        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] int key)
        {
            return SingleResult.Create(db.Rents.Where(m => m.Id == key).Select(m => m.Client));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RentExists(int key)
        {
            return db.Rents.Count(e => e.Id == key) > 0;
        }
    }
}
