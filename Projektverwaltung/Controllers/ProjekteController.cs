using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Projektverwaltung.Database;

namespace Projektverwaltung.Controllers
{
    public class ProjekteController : Controller
    {
        private ProjektverwaltungEntities db = new ProjektverwaltungEntities();

        private SelectList StatusList = new SelectList(
                new Dictionary<int, string>
                {
                    { 1, "In Planung" },
                    { 2, "Geplant" },
                    { 3, "Bewilligt" },
                    { 4, "In Arbeit" },
                    { 5, "Abgeschlossen" }
                }, "Key", "Value");

        private SelectList PrioList = new SelectList(
                new Dictionary<int, string>
                {
                    { 1, "1 - Tief" },
                    { 2, "2 - Mittel" },
                    { 3, "3 - Hoch" }
                }, "Key", "Value");

        // GET: Projekte
        public ActionResult Index()
        {
            var projekt = db.Projekt.Include(p => p.Mitarbeiter).Include(p => p.Vorgehensmodell);
            ViewBag.StatusList = StatusList;
            ViewBag.PrioList = PrioList;
            return View(projekt.ToList());
        }

        // GET: Projekte/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projekt projekt = db.Projekt.Find(id);
            if (projekt == null)
            {
                return HttpNotFound();
            }

            ViewBag.StatusList = StatusList;
            ViewBag.PrioList = PrioList;

            return View(projekt);
        }

        // GET: Projekte/Create
        public ActionResult Create()
        {
            ViewBag.projektleiter_id = new SelectList(db.Mitarbeiter, "id", "name");
            ViewBag.vorgehensmodell_id = new SelectList(db.Vorgehensmodell, "id", "name");

            ViewBag.StatusList = this.StatusList;
            ViewBag.PrioList = this.PrioList;

            return View();
        }

        // POST: Projekte/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,beschreibung,status,prioritaet,startdatum_geplant,enddatum_geplant,startdatum_effektiv,enddatum_effektiv,fortschritt,dokument_link,projektleiter_id,vorgehensmodell_id")] Projekt projekt)
        {
            if (ModelState.IsValid)
            {
                db.Projekt.Add(projekt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.projektleiter_id = new SelectList(db.Mitarbeiter, "id", "name", projekt.projektleiter_id);
            ViewBag.vorgehensmodell_id = new SelectList(db.Vorgehensmodell, "id", "name", projekt.vorgehensmodell_id);
            return View(projekt);
        }

        // GET: Projekte/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projekt projekt = db.Projekt.Find(id);
            if (projekt == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusList = this.StatusList;
            ViewBag.PrioList = this.PrioList;
            ViewBag.projektleiter_id = new SelectList(db.Mitarbeiter, "id", "name", projekt.projektleiter_id);
            ViewBag.vorgehensmodell_id = new SelectList(db.Vorgehensmodell, "id", "name", projekt.vorgehensmodell_id);
            return View(projekt);
        }

        // POST: Projekte/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,beschreibung,status,prioritaet,startdatum_geplant,enddatum_geplant,startdatum_effektiv,enddatum_effektiv,fortschritt,dokument_link,projektleiter_id,vorgehensmodell_id")] Projekt projekt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projekt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projektleiter_id = new SelectList(db.Mitarbeiter, "id", "name", projekt.projektleiter_id);
            ViewBag.vorgehensmodell_id = new SelectList(db.Vorgehensmodell, "id", "name", projekt.vorgehensmodell_id);
            return View(projekt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
