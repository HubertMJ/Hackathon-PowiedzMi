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
    public class UzytkowniciesController : ApiController
    {
        private PowiedzMiEntities1 db = new PowiedzMiEntities1();

        // GET: api/Uzytkownicies
        public IQueryable<Uzytkownicy> GetUzytkownicy()
        {
            return db.Uzytkownicy;
        }

        // GET: api/Uzytkownicies/5
        [ResponseType(typeof(Uzytkownicy))]
        public IHttpActionResult GetUzytkownicy(int id)
        {
            Uzytkownicy uzytkownicy = db.Uzytkownicy.Find(id);
            if (uzytkownicy == null)
            {
                return NotFound();
            }

            return Ok(uzytkownicy);
        }

        // PUT: api/Uzytkownicies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUzytkownicy(int id, Uzytkownicy uzytkownicy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uzytkownicy.id)
            {
                return BadRequest();
            }

            db.Entry(uzytkownicy).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UzytkownicyExists(id))
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

        [Route("api/zaloguj")]
        [HttpGet]
        public int login(string username, string password)
        {
            var user = db.Uzytkownicy.FirstOrDefault(x => x.imie.ToLower() == username.ToLower());
            if (user == null)
                return 0;

            if (user.haslo == password)
                return user.id;
            return -1;
        }

        // POST: api/Uzytkownicies
        [ResponseType(typeof(Uzytkownicy))]
        public int PostUzytkownicy(Uzytkownicy uzytkownicy)
        {
            if (!ModelState.IsValid)
            {
                return -1;
            }

            var _uzytkownicy = db.Uzytkownicy.FirstOrDefault(x => x.imie.ToLower() == uzytkownicy.imie.ToLower());
            if (_uzytkownicy != null)
            {
                return -1;
            }

            db.Uzytkownicy.Add(uzytkownicy);
            db.SaveChanges();

            return 1;
        }



     // DELETE: api/Uzytkownicies/5
     [ResponseType(typeof(Uzytkownicy))]
        public IHttpActionResult DeleteUzytkownicy(int id)
        {
            Uzytkownicy uzytkownicy = db.Uzytkownicy.Find(id);
            if (uzytkownicy == null)
            {
                return NotFound();
            }

            db.Uzytkownicy.Remove(uzytkownicy);
            db.SaveChanges();

            return Ok(uzytkownicy);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UzytkownicyExists(int id)
        {
            return db.Uzytkownicy.Count(e => e.id == id) > 0;
        }
    }
}