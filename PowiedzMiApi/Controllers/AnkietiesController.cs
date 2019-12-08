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
using PowiedzMiApi.Models;
using PowiedzMiDataAccess;

namespace PowiedzMiApi.Controllers
{
    public class AnkietiesController : ApiController
    {
        private PowiedzMiEntities1 db = new PowiedzMiEntities1();

        // GET: api/Ankieties
        public IQueryable<Ankiety> GetAnkiety()
        {
            return db.Ankiety;
        }

        // GET: api/Ankieties/5
        [ResponseType(typeof(Ankiety))]
        public IHttpActionResult GetAnkiety(int id)
        {
            Ankiety ankiety = db.Ankiety.Find(id);
            if (ankiety == null)
            {
                return NotFound();
            }

            return Ok(ankiety);
        }
        [Route("api/pobierzPytania")]
        [HttpGet]
        public List<String> pobierzPytania()
        {
            List<String> temp = new List<String>();
            foreach (var x in db.Pytania)
            {
                temp.Add(x.tresc);
            }
            return temp;
        }
        [HttpGet]
        [Route("api/WynikAnkiety")]
        public WynikAnkiety WynikAnkiety(int userid, DateTime date)
        {
            WynikAnkiety wynik = new WynikAnkiety();
            wynik.id_uzytkownika = userid;
            var ankiety = db.Ankiety.Where(x => x.id_uzytkownika == userid && x.data == date );
            foreach (var x in ankiety) {
                wynik.komentarz = x.komentarz;
                wynik.id_ankiety = x.id;
            }
            var odpowiedzi = db.PytanieWAnkiecie.Where(x => x.id_ankiety == wynik.id_ankiety);
            foreach (var x in odpowiedzi)
            {
                wynik.odpowiedzi.Add(x.odpowiedz);
                wynik.ocena += x.odpowiedz;
            }
            return wynik;
        }
        [HttpGet]
        [Route("api/MojeKregi")]
        public List<int> MojeKregi(int userid)
        {
            List<int> mojekregi = new List<int>();

            var kregi = db.CzlonekKregu.Where(x => x.id_uzytkownika == userid);
            foreach (var x in kregi)
            {
                mojekregi.Add(x.id_kregu);
            }

            return mojekregi;
        }
        [HttpGet]
        [Route("api/UzytkownicyKregu")]
        public List<int> UzytkownicyKregu(int kregid)
        {
            List<int> uzytkownicyKregu = new List<int>();

            var kregi = db.CzlonekKregu.Where(x => x.id_kregu == kregid);
            foreach (var x in kregi)
            {
                uzytkownicyKregu.Add(x.id_uzytkownika);
            }

            return uzytkownicyKregu;
        }
        [HttpGet]
        [Route("api/AnikietyZKregu")]
        public List<WynikAnkiety> AnikietyZKregu(int kregid)
        {
            List<WynikAnkiety> wynik = new List<WynikAnkiety>();
            List<int> users = new List<int>();
            users = UzytkownicyKregu(kregid);
            var i = 0;
            foreach(var x in users)
            {
                wynik.Add(new WynikAnkiety());
                wynik[i] = WynikAnkiety(x, DateTime.Today);
                i-=-1;
            }




            return wynik;
        }
        [HttpPost]
        [Route("api/odpowiedzWAnkiecie")]
        public int odpowiedzWAnkiecie(int userid, [FromUri]String answer, String com){
            int i = 1;
            List<int> _answer = new List<int>();
            var tempo = answer.Split(',');
            foreach (var j in tempo) {
                _answer.Add(Convert.ToInt32(j));
                    }


            var _ankieta = db.Ankiety.FirstOrDefault(x => x.id_uzytkownika == userid);
            if (_ankieta != null && _ankieta.data == DateTime.Today)
                return -1;
            if (_ankieta != null && _ankieta.data != DateTime.Today || _ankieta == null)
            {
                _ankieta = new Ankiety();
                _ankieta.id_uzytkownika = userid;
                _ankieta.data = DateTime.Now;
                _ankieta.komentarz = com;
                db.Ankiety.Add(_ankieta);

                var temp = new PytanieWAnkiecie();
                temp.id_ankiety = _ankieta.id;

                foreach (var u in _answer)
                {
                                    
                    temp.id_pytanie = i;
                    temp.odpowiedz = u;
                    db.PytanieWAnkiecie.Add(temp);
                    db.SaveChanges();
                    i -= -1;
                }
                
                db.SaveChanges();
                return 1;
            }
            else
                return -1;
        }

        // PUT: api/Ankieties/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAnkiety(int id, Ankiety ankiety)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ankiety.id)
            {
                return BadRequest();
            }

            db.Entry(ankiety).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnkietyExists(id))
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

        // POST: api/Ankieties
        [ResponseType(typeof(Ankiety))]
        public IHttpActionResult PostAnkiety(Ankiety ankiety)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ankiety.Add(ankiety);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ankiety.id }, ankiety);
        }

        // DELETE: api/Ankieties/5
        [ResponseType(typeof(Ankiety))]
        public IHttpActionResult DeleteAnkiety(int id)
        {
            Ankiety ankiety = db.Ankiety.Find(id);
            if (ankiety == null)
            {
                return NotFound();
            }

            db.Ankiety.Remove(ankiety);
            db.SaveChanges();

            return Ok(ankiety);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnkietyExists(int id)
        {
            return db.Ankiety.Count(e => e.id == id) > 0;
        }
    }
}