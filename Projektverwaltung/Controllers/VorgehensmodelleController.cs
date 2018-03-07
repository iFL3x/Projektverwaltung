// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VorgehensmodelleController.cs" author="Fabrice Koenig">
//   Copyright (c) Fabrice Koenig Ltd. All rights reserved.
// </copyright>
// <summary>
//   Controller class user to control the Vorgehensmodell Data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Projektverwaltung.Database;

namespace Projektverwaltung.Controllers
{
    /// <summary> The VorgehensmodelleController controlls the Vorgehensmodell Data for the application and consists of different Actions and methods to do so.</summary>
    public class VorgehensmodelleController : Controller
    {
        //Database context object
        private ProjektverwaltungEntities db = new ProjektverwaltungEntities();

        //State List: Populated to views to display the state
        private SelectList StatusList = new SelectList(
                new Dictionary<int, string>
                {
                    { 1, "Aktiv" },
                    { 2, "Inaktiv" }
                }, "Key", "Value");


        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>List of all Projects</returns>
        /// Example: GET: Vorgehensmodelle
        public ActionResult Index()
        {
            //Populate State List Data to ViewData
            ViewBag.StatusList = StatusList;

            //Return View and pass all Vorgehensmodelle
            return View(db.Vorgehensmodell.ToList());
        }

        /// <summary>
        /// Details Action for Vorgehensmodelle
        /// </summary>
        /// <param name="id">Vorgehensmodell ID</param>
        /// <returns>returns the detail View</returns>
        /// Example: GET: Vorgehensmodelle/Details/5
        public ActionResult Details(int? id)
        {
            //Check if an ID was set and if not, show a Bad-Request-Error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Search the employee in the database
            Vorgehensmodell vorgehensmodell = db.Vorgehensmodell.Find(id);
            if (vorgehensmodell == null)
            {
                return HttpNotFound();
            }
            //Return Details View
            return View(vorgehensmodell);
        }

        /// <summary>
        /// Create Action for Vorgehensmodelle
        /// </summary>
        /// <returns>returns the create view</returns>
        /// Example: GET: Vorgehensmodelle/Create
        public ActionResult Create()
        {
            //Populate Viewbag Data
            ViewBag.StatusList = StatusList;

            //Return view
            return View();
        }

        /// <summary>
        /// Create Action that is called by a Post on the create view.
        /// Tries to save the changes posted back to the database
        /// </summary>
        /// <param name="vorgehensmodell">Vorgehensmodell Object</param>
        /// <returns>Create view with changed model</returns>
        /// Example POST: Vorgehensmodelle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,status")] Vorgehensmodell vorgehensmodell)
        {
            //Check if the model is still valid
            if (ModelState.IsValid)
            {
                //Add and save
                db.Vorgehensmodell.Add(vorgehensmodell);
                db.SaveChanges();

                //Return to the index action in case saving succeeded
                return RedirectToAction("Index");
            }

            //Return the create view with the updated data
            return View(vorgehensmodell);
        }

        /// <summary>
        /// Edit Action for Vorgehensmodelle
        /// </summary>
        /// <param name="id"></param>
        /// <returns>edit view</returns>
        /// Example: GET: Vorgehensmodelle/Edit/5
        public ActionResult Edit(int? id)
        {
            //Check if an ID was set and if not, show a Bad-Request-Error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Search the vorgehensmodell in the database
            Vorgehensmodell vorgehensmodell = db.Vorgehensmodell.Find(id);

            //If no employee was found with the given ID, show an error
            if (vorgehensmodell == null)
            {
                return HttpNotFound();
            }

            //Populate Viewbag Data
            ViewBag.StatusList = StatusList;

            //Return Edit View
            return View(vorgehensmodell);
        }

        /// <summary>
        /// Edit Action that is called by a Post on the edit view.
        /// Tries to save the changes posted back to the database
        /// </summary>
        /// <param name="vorgehensmodell"></param>
        /// <returns>Edit view with changed model</returns>
        /// Example: POST: Vorgehensmodelle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vorgehensmodell vorgehensmodell)
        {
            //Check if the Model (Data) is still valid
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

                //Return to Index action in case saving succeeded
                return RedirectToAction("Index");
            }
            //Return to Edit View
            return View(vorgehensmodell);
        }

        /// <summary>
        /// Delete Action for Vorgehensmodelle
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delte View</returns>
        public ActionResult Delete(int? id)
        {
            //Check if the id was set, if not show an error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Search the vorgehensmodell in the database
            Vorgehensmodell vorgehensmodell = db.Vorgehensmodell.Find(id);

            //If no vorgehensmodell was found with the given ID, show an error
            if (vorgehensmodell == null)
            {
                return HttpNotFound();
            }

            //Return Delete View
            return View(vorgehensmodell);
        }

        /// <summary>
        /// Delete Action that is called by a Post on the delete view.
        /// Tries to remove an employee from the context and delete it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// POST: Vorgehensmodelle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Search the vorgehensmodell in the database
            Vorgehensmodell vorgehensmodell = db.Vorgehensmodell.Find(id);

            //Remove it from the context
            db.Vorgehensmodell.Remove(vorgehensmodell);

            //Save Changes on the Database
            db.SaveChanges();

            //Redirect to the Index View
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action that adds phases to a given vorgehensmodell
        /// </summary>
        /// <param name="vorgehensmodellId">ID of the Vorgehensmodell</param>
        /// <returns>Redirect to Edit View</returns>
        public ActionResult AddPhase(int vorgehensmodellId)
        {
            //Add new Vorgehensmodell
            db.VorgehensmodellPhase.Add(new VorgehensmodellPhase() {
                vorgehensmodell_id = vorgehensmodellId,
                name = "Neue Phase"
            });

            //Try to save changes or throw exception
            try
            {
                db.SaveChanges();
            } catch (Exception e) { throw e; }

            //Redirect to the Edit Project View
            return this.RedirectToAction("Edit", new { id = vorgehensmodellId });
        }

        /// <summary>
        /// Action that removes a phase from a vorgehensmodell
        /// </summary>
        /// <param name="phaseId">ID of the phase</param>
        /// <param name="vorgehensmodellId">ID of the vorgehensmodell</param>
        /// <returns>Redirect to Edit View</returns>
        public ActionResult RemovePhase(int phaseId, int vorgehensmodellId)
        {
            //Search phase object in the database
            var phase = db.VorgehensmodellPhase.FirstOrDefault(p => p.id == phaseId);

            //Remove phase from the context
            db.VorgehensmodellPhase.Remove(phase);

            //Try to save changes or throw exception
            try
            {
                db.SaveChanges();
            } catch (Exception e){ throw e;  }

            //Return to Edit Project View
            return this.RedirectToAction("Edit", new { id = vorgehensmodellId});
        }

        //Database Displose Method
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
