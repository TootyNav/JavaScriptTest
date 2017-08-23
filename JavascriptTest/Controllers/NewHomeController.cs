using JavascriptTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace JavascriptTest.Controllers
{
    public class NewHomeController : Controller
    {
        private TechTestEntities dc = new TechTestEntities();

        // GET: NewHome
        public ActionResult Index()
        {
            return View(/*db.People.ToList()*/);
        }

        // GET every person
        public ActionResult GetPeople()
        {
            var data = dc.People.Include(c => c.Colours)
                .OrderByDescending(x => x.PersonId).AsEnumerable()
                .Select(i => new {
                    i.PersonId,
                    FullName = i.FirstName + " " + i.LastName,
                    i.IsAuthorised,
                    i.IsEnabled,
                    Colours = string.Join(", ", i.Colours.OrderBy(x => x.Name).Select(x => x.Name))
                })
                .ToList();
            
            return Json(new { data = data}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.id = id;
            
            return View();
        }

        public ActionResult GetDetails(int id)
        {
            var data = dc.People.Include(c => c.Colours).AsEnumerable()
            .Select(i => new
            {
                i.PersonId,
                FullName = i.FirstName + " " + i.LastName,
                i.IsAuthorised,
                i.IsEnabled,
                Colours = i.Colours.Select(x => x.Name)
            }).SingleOrDefault(p => p.PersonId == id);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(Array data)
        {
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}