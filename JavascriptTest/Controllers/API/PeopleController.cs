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
using JavascriptTest.Models;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace JavascriptTest.Controllers.API
{
    public class PeopleController : ApiController
    {
        private TechTestEntities db = new TechTestEntities();

        //GET: /api/People
        //public ActionResult GetPeople()
        //{

        //    IList<Person> data = db.People.ToList();
        //    //return Json(data, JsonRequestBehavior.AllowGet);


        //    return new JsonResult()
        //    {
        //        Data = data,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}



        public dynamic GetPeople()
        {
 
            var data = db.People.Select(i => new {
                i.PersonId, i.FirstName, i.LastName, i.IsAuthorised, i.IsValid, i.IsEnabled }).ToList();

            return new JsonResult()
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }








        //string[] per = new string[] { "s", "b" };




        //// GET: api/People
        //public IQueryable<Person> GetPeople()
        //{
        //    return db.People;
        //}

        // GET: api/People/5
        [ResponseType(typeof(Person))]
        public IHttpActionResult GetPerson(int id)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/People/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPerson(int id, Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.PersonId)
            {
                return BadRequest();
            }

            db.Entry(person).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/People
        [ResponseType(typeof(Person))]
        public IHttpActionResult PostPerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.People.Add(person);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = person.PersonId }, person);
        }

        // DELETE: api/People/5
        [ResponseType(typeof(Person))]
        public IHttpActionResult DeletePerson(int id)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            db.People.Remove(person);
            db.SaveChanges();

            return Ok(person);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonExists(int id)
        {
            return db.People.Count(e => e.PersonId == id) > 0;
        }
    }
}