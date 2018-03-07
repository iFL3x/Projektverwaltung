// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MitarbeiterController.cs" author="Fabrice Koenig">
//   Copyright (c) Fabrice Koenig Ltd. All rights reserved.
// </copyright>
// <summary>
//   Controller class user to control the Employee Data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using Projektverwaltung.Database;
using Projektverwaltung.Models;

namespace Projektverwaltung.Controllers
{
    /// <summary> The MitarbeiterController controlls the employee Data for the application and consists of different Actions and methods to do so.</summary>
    public class MitarbeiterController : Controller
    {
        //Database context object
        private ProjektverwaltungEntities db = new ProjektverwaltungEntities();


        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>List of all Employees</returns>
        /// Example: GET: Mitarbeiter
        public ActionResult Index()
        {
            return View(db.Mitarbeiter.ToList());
        }


        /// <summary>
        /// Details Action for Employees
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>returns the detail View</returns>
        /// Example: GET: Mitarbeiter/Details/5
        public ActionResult Details(int? id)
        {
            //Check if an ID was set and if not, show a Bad-Request-Error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Search the employee in the database
            Mitarbeiter mitarbeiter = db.Mitarbeiter.Find(id);
            if (mitarbeiter == null)
            {
                return HttpNotFound();
            }

            //Return View
            return View(mitarbeiter);
        }

        /// <summary>
        /// Create Action for Employees
        /// </summary>
        /// <returns>returns the create view</returns>
        /// Example: GET: Mitarbeiter/Create
        public ActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Create Action that is called by a Post on the create view.
        /// Tries to save the changes posted back to the database
        /// </summary>
        /// <param name="mitarbeiter"></param>
        /// <returns>Create view with changed model</returns>
        /// Example POST: Mitarbeiter/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,vorname,abteilung,pensum")] Mitarbeiter mitarbeiter)
        {
            //Check if the Model (Data) is still valid
            if (ModelState.IsValid)
            {
                //Add and save
                db.Mitarbeiter.Add(mitarbeiter);
                db.SaveChanges();

                //Return to the index action in case saving succeeded
                return RedirectToAction("Index");
            }

            //Return the create view with the updated data
            return View(mitarbeiter);
        }



        /// <summary>
        /// Edit Action for Employees
        /// </summary>
        /// <param name="id"></param>
        /// <returns>edit view</returns>
        /// Example: GET: Mitarbeiter/Edit/5
        public ActionResult Edit(int? id)
        {
            //Check if an ID was set and if not, show a Bad-Request-Error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Search the employee in the database
            Mitarbeiter mitarbeiter = new Mitarbeiter(); 
            mitarbeiter = db.Mitarbeiter.Find(id);

            //If no employee was found with the given ID, show an error
            if (mitarbeiter == null)
            {
                return HttpNotFound();
            }

            //Automatically map the properties of "Mitarbeiter" to MitarbeiterViewMode
            var model = Mapper.Map<MitarbeiterViewModel>(mitarbeiter);

            //Populate Employee Functions
            this.ViewBag.AllFunctions = this.db.Funktion.ToList();

            //Return Edit view
            return View(model);
        }

        /// <summary>
        /// Edit Action that is called by a Post on the edit view.
        /// Tries to save the changes posted back to the database
        /// </summary>
        /// <param name="mitarbeiter"></param>
        /// <returns>Edit view with changed model</returns>
        /// Example: POST: Mitarbeiter/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,vorname,abteilung,pensum")] Mitarbeiter mitarbeiter)
        {
            //Check if the Model (Data) is still valid
            if (ModelState.IsValid)
            {
                //Add and save
                db.Entry(mitarbeiter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //Return Edit View
            return View(mitarbeiter);
        }

        /// <summary>
        /// Delete Action for Employees
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
            //Search the employee in the database
            Mitarbeiter mitarbeiter = db.Mitarbeiter.Find(id);

            //If no employee was found with the given ID, show an error
            if (mitarbeiter == null)
            {
                return HttpNotFound();
            }

            //Return Delete View
            return View(mitarbeiter);
        }

        /// <summary>
        /// Delete Action that is called by a Post on the delete view.
        /// Tries to remove an employee from the context and delete it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// POST: Mitarbeiter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Search the employee in the database
            Mitarbeiter mitarbeiter = db.Mitarbeiter.Find(id);

            //Remove it from the context
            db.Mitarbeiter.Remove(mitarbeiter);

            //Save Changes on the Database
            db.SaveChanges();

            //Redirect to the Index View
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action that Removes Functions from a given employee
        /// </summary>
        /// <param name="functionId">ID of the Function</param>
        /// <param name="employeeId">ID of the Employee</param>
        /// <returns>Redirect to Edit View</returns>
        public ActionResult RemoveFunctionFromEmployee(int functionId, int employeeId)
        {
            //Search the database for the employee-function object
            var mf = db.MitarbeiterFunktion.SingleOrDefault(m => m.funktion_id == functionId && m.mitarbeiter_id == employeeId);

            if (mf != null)
            {
                //Remove the object from the Context
                db.MitarbeiterFunktion.Remove(mf);

                //Save changes on the database
                db.SaveChanges();
            }

            // Go back to the edit view
            return this.RedirectToAction("Edit", new { id = employeeId });
        }


        /// <summary>
        /// Action that Adds Functions to a given employee
        /// </summary>
        /// <param name="functionId">ID of the function</param>
        /// <param name="employeeId">ID of the employee</param>
        /// <returns></returns>
        public ActionResult AddFunctionToEmployee(int functionId, int employeeId)
        {  
            //Add Function to the context table
            db.MitarbeiterFunktion.Add(new MitarbeiterFunktion() { funktion_id = functionId, mitarbeiter_id = employeeId });

            //Save changes on database
            db.SaveChanges();

            //Redirect to the edit Action
            return this.RedirectToAction("Edit", new { id = employeeId });
        }

        /// <summary></summary>
        /// <param name="disposing"></param>
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
