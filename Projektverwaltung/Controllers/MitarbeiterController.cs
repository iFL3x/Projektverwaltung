using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Projektverwaltung.Database;
using Projektverwaltung.Models;

namespace Projektverwaltung.Controllers
{
    public class MitarbeiterController : Controller
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

        // GET: Mitarbeiter
        public ActionResult Index()
        {
            return View(db.Mitarbeiter.ToList());
        }

        // GET: Mitarbeiter/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Mitarbeiter mitarbeiter = db.Mitarbeiter.Find(id);
            if (mitarbeiter == null)
            {
                return HttpNotFound();
            }

            ViewBag.RoleList = StatusList;

            return View(mitarbeiter);
        }

        // GET: Mitarbeiter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mitarbeiter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,vorname,abteilung,pensum")] Mitarbeiter mitarbeiter)
        {
            if (ModelState.IsValid)
            {
                db.Mitarbeiter.Add(mitarbeiter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mitarbeiter);
        }



        // GET: Mitarbeiter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Mitarbeiter mitarbeiter = new Mitarbeiter(); // is that line really necessary?
            mitarbeiter = db.Mitarbeiter.Find(id);


            if (mitarbeiter == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<MitarbeiterViewModel>(mitarbeiter);
            this.ViewBag.AllFunctions = this.db.Funktion.ToList();

            return View(model);
        }

        // POST: Mitarbeiter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,vorname,abteilung,pensum")] Mitarbeiter mitarbeiter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mitarbeiter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mitarbeiter);
        }

        // GET: Mitarbeiter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mitarbeiter mitarbeiter = db.Mitarbeiter.Find(id);
            if (mitarbeiter == null)
            {
                return HttpNotFound();
            }
            return View(mitarbeiter);
        }

        // POST: Mitarbeiter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mitarbeiter mitarbeiter = db.Mitarbeiter.Find(id);
            db.Mitarbeiter.Remove(mitarbeiter);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFunctionFromEmployee(int functionId, int employeeId)
        {
            var mf = db.MitarbeiterFunktion.SingleOrDefault(m => m.funktion_id == functionId && m.mitarbeiter_id == employeeId);

            if (mf != null)
            {
                db.MitarbeiterFunktion.Remove(mf);
                db.SaveChanges();
            }

            // and go back to edit
            return this.RedirectToAction("Edit", new { id = employeeId });
        }

        public ActionResult AddFunctionToEmployee(int functionId, int employeeId)
        {  
            db.MitarbeiterFunktion.Add(new MitarbeiterFunktion() { funktion_id = functionId, mitarbeiter_id = employeeId });
            db.SaveChanges();

            return this.RedirectToAction("Edit", new { id = employeeId });
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
