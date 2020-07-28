using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Wiki;

namespace Wiki.Areas.CMOS.Controllers
{
    public class CMOSApiController : ApiController
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public IQueryable<CMOSOrder> GetCMOSOrder()
        {
            return db.CMOSOrder;
        }

        [ResponseType(typeof(CMOSOrder))]
        public async Task<IHttpActionResult> GetCMOSOrder(int id)
        {
            CMOSOrder cMOSOrder = await db.CMOSOrder.FindAsync(id);
            if (cMOSOrder == null)
            {
                return NotFound();
            }

            return Ok(cMOSOrder);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCMOSOrder(int id, CMOSOrder cMOSOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cMOSOrder.id)
            {
                return BadRequest();
            }

            db.Entry(cMOSOrder).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CMOSOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(CMOSOrder))]
        public async Task<IHttpActionResult> PostCMOSOrder(CMOSOrder cMOSOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CMOSOrder.Add(cMOSOrder);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cMOSOrder.id }, cMOSOrder);
        }

        [ResponseType(typeof(CMOSOrder))]
        public async Task<IHttpActionResult> DeleteCMOSOrder(int id)
        {
            CMOSOrder cMOSOrder = await db.CMOSOrder.FindAsync(id);
            if (cMOSOrder == null)
            {
                return NotFound();
            }

            db.CMOSOrder.Remove(cMOSOrder);
            await db.SaveChangesAsync();

            return Ok(cMOSOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CMOSOrderExists(int id)
        {
            return db.CMOSOrder.Count(e => e.id == id) > 0;
        }
    }
}