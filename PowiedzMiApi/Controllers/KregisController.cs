using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PowiedzMiDataAccess;

namespace PowiedzMiApi.Controllers
{
    public class KregisController : ApiController
    {
        private PowiedzMiEntities1 db = new PowiedzMiEntities1();

        // GET: api/Kregis
        public IQueryable<Kregi> GetKregi()
        {
            return db.Kregi;
        }

        // GET: api/Kregis/5
        [ResponseType(typeof(Kregi))]
        public IHttpActionResult GetKregi(int id)
        {
            Kregi kregi = db.Kregi.Find(id);
            if (kregi == null)
            {
                return NotFound();
            }

            return Ok(kregi);
        }

        // PUT: api/Kregis/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKregi(int id, Kregi kregi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kregi.id)
            {
                return BadRequest();
            }

            db.Entry(kregi).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KregiExists(id))
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

        // POST: api/Kregis
        [Route("api/kregis/utworz")]
        [HttpGet]
        public int PostKregi(Kregi kregi, int userid)
        {
            if (!ModelState.IsValid)
            {
                return 0;
            }

            var _kregi = db.Kregi.FirstOrDefault(x => x.nazwa == kregi.nazwa);
            if (_kregi == null)
            {

                db.Kregi.Add(kregi);

                CzlonekKregu czlonek = new CzlonekKregu();
                czlonek.id_kregu = kregi.id;
                czlonek.id_uzytkownika = userid;
                db.CzlonekKregu.Add(czlonek);

                db.SaveChanges();
                return 1;
            }



            return 0;
        }

        // DELETE: api/Kregis/5
        [ResponseType(typeof(Kregi))]
        public IHttpActionResult DeleteKregi(int id)
        {
            Kregi kregi = db.Kregi.Find(id);
            if (kregi == null)
            {
                return NotFound();
            }

            db.Kregi.Remove(kregi);
            db.SaveChanges();

            return Ok(kregi);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KregiExists(int id)
        {
            return db.Kregi.Count(e => e.id == id) > 0;
        }
    }
}