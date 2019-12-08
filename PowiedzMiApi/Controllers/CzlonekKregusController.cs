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
    public class CzlonekKregusController : ApiController
    {
        private PowiedzMiEntities1 db = new PowiedzMiEntities1();

        // GET: api/CzlonekKregus
        public IQueryable<CzlonekKregu> GetCzlonekKregu()
        {
            return db.CzlonekKregu;
        }

        // GET: api/CzlonekKregus/5
        [ResponseType(typeof(CzlonekKregu))]
        public IHttpActionResult GetCzlonekKregu(int id)
        {
            CzlonekKregu czlonekKregu = db.CzlonekKregu.Find(id);
            if (czlonekKregu == null)
            {
                return NotFound();
            }

            return Ok(czlonekKregu);
        }

        // PUT: api/CzlonekKregus/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCzlonekKregu(int id, CzlonekKregu czlonekKregu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != czlonekKregu.id)
            {
                return BadRequest();
            }

            db.Entry(czlonekKregu).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CzlonekKreguExists(id))
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

        // POST: api/CzlonekKregus
        [ResponseType(typeof(CzlonekKregu))]
        [Route("api/dodajdokregu")]
        [HttpGet]
        public int PostCzlonekKregu(string nazwa, int userid)
        {
            if (!ModelState.IsValid)
            {
                return -1;
            }
            var _krag = db.Kregi.FirstOrDefault(x => x.nazwa.ToLower() == nazwa.ToLower());

            if (_krag != null)
            {
                var count = db.CzlonekKregu.ToList().Count;
                if (count != 0)
                {
                    foreach (CzlonekKregu x in db.CzlonekKregu.ToList())
                    {
                        if (x.id_kregu == _krag.id)
                        {
                            if (x.id_uzytkownika != userid)
                            {
                                var _czlonekKregu = new CzlonekKregu();
                                _czlonekKregu.id_kregu = _krag.id;
                                _czlonekKregu.id_uzytkownika = userid;
                                db.CzlonekKregu.Add(_czlonekKregu);
                                db.SaveChanges();
                                return 1;
                            }
                            else
                                return -1;
                        }
                    }
                }
                else
                {
                    var _czlonekKregu = new CzlonekKregu();
                    _czlonekKregu.id_kregu = _krag.id;
                    _czlonekKregu.id_uzytkownika = userid;
                    db.CzlonekKregu.Add(_czlonekKregu);
                    db.SaveChanges();
                    return 1;
                }
            }
            return -1;
        }

        // DELETE: api/CzlonekKregus/5
        [ResponseType(typeof(CzlonekKregu))]
        public IHttpActionResult DeleteCzlonekKregu(int id)
        {
            CzlonekKregu czlonekKregu = db.CzlonekKregu.Find(id);
            if (czlonekKregu == null)
            {
                return NotFound();
            }

            db.CzlonekKregu.Remove(czlonekKregu);
            db.SaveChanges();

            return Ok(czlonekKregu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CzlonekKreguExists(int id)
        {
            return db.CzlonekKregu.Count(e => e.id == id) > 0;
        }
    }
}