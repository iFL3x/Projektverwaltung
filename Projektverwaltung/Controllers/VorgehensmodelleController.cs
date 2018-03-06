using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Projektverwaltung.Database;
using Projektverwaltung.Models;
using AutoMapper;

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
        public ActionResult Create([Bind(Include = "id,name,status")] Vorgehensmodell vorgehensmodell)
        {
            if (ModelState.IsValid)
            {
                db.Vorgehensmodell.Add(vorgehensmodell);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vorgehensmodell);
        }

        // GET: Vorgehensmodelle/Edit/5
        public ActionResult Edit(int? id)
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

            //var model = Mapper.Map<VorgehensmodellViewModel>(vorgehensmodell);

            ViewBag.StatusList = StatusList;
            return View(vorgehensmodell);
        }

        // POST: Vorgehensmodelle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vorgehensmodell vorgehensmodell)
        {
            if (ModelState.IsValid)
            {

                //Loop through the phases and update the entries in the db table
                foreach(var item in vorgehensmodell.VorgehensmodellPhase)
                {
                    var dbitem = db.VorgehensmodellPhase.FirstOrDefault(s => s.id == item.id);
                    dbitem.name = item.name;
                    dbitem.beschreibung = item.beschreibung;
                }

                //Clear the table on the model so we don't get conflicts here
                vorgehensmodell.VorgehensmodellPhase.Clear();

                try
                {
                    //Update vorgehensmodell table and save changes
                    db.Entry(vorgehensmodell).State = EntityState.Modified;
                    db.SaveChanges();
                } catch(Exception e)
                {
                    throw e;
                }

                
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

        public ActionResult AddPhase(int vorgehensmodellId)
        {

            db.VorgehensmodellPhase.Add(new VorgehensmodellPhase() {
                vorgehensmodell_id = vorgehensmodellId,
                name = "Neue Phase"
            });
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            return this.RedirectToAction("Edit", new { id = vorgehensmodellId });
        }

        public ActionResult RemovePhase(int phaseId, int vorgehensmodellId)
        {

            var phase = db.VorgehensmodellPhase.FirstOrDefault(p => p.id == phaseId);
            db.VorgehensmodellPhase.Remove(phase);
            try
            {
                db.SaveChanges();
            } catch (Exception e)
            {
                throw e;
            }

            return this.RedirectToAction("Edit", new { id = vorgehensmodellId});
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
