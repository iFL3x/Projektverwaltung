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
    public class VorgehensmodelleController : Controller
    {
        private ProjektverwaltungEntities db = new ProjektverwaltungEntities();

        private SelectList StatusList = new SelectList(
                new Dictionary<int, string>
                {
                    { 1, "Aktiv" },
                    { 2, "Inaktiv" }
                }, "Key", "Value");


        // GET: Vorgehensmodelle
        public ActionResult Index()
        {
            ViewBag.StatusList = StatusList;

            return View(db.Vorgehensmodell.ToList());
        }

        // GET: Vorgehensmodelle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vorgehensmodell vorgehensmodell = db.Vorgehensmodell.Find(id);
            if (vorgehensmodell == null)
            {
                return HttpNotFound();
            }

            //model.phasen = db.VorgehensmodellPhase.Where(p => p.vorgehensmodell_id == vorgehensmodell.id).ToList();

            return View(vorgehensmodell);
        }

        // GET: Vorgehensmodelle/Create
        public ActionResult Create()
        {     
            ViewBag.StatusList = StatusList;

            return View();
        }

        // POST: Vorgehensmodelle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vorgehensmodell modell)
        {
            if (ModelState.IsValid)
            {
                db.Vorgehensmodell.Add(modell);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modell);
        }

        // GET: Vorgehensmodelle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new Vorgehensmodell();
            model = db.Vorgehensmodell.Find(id);

            ViewBag.StatusList = StatusList;

            return View(model);
        }


        // POST: Vorgehensmodelle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vorgehensmodell vorgehensmodell)
        {
            if (ModelState.IsValid)
            {

                // Save data
                db.Entry(vorgehensmodell).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vorgehensmodell);
        }

        // GET: Vorgehensmodelle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vorgehensmodell vorgehensmodell = db.Vorgehensmodell.Find(id);
            if (vorgehensmodell == null)
            {
                return HttpNotFound();
            }
            return View(vorgehensmodell);
        }

        // POST: Vorgehensmodelle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vorgehensmodell vorgehensmodell = db.Vorgehensmodell.Find(id);
            db.Vorgehensmodell.Remove(vorgehensmodell);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddPhase()
        {
            var phase = new VorgehensmodellPhase();
            
            // Add phase to the List  vorgehensmodell.VorgehensmodellPhase


            return PartialView("~/Views/Vorgehensmodelle/Phase.cshtml", new VorgehensmodellPhase());
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
