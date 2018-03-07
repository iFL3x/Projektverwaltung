// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjekteController.cs" author="Fabrice Koenig">
//   Copyright (c) Fabrice Koenig Ltd. All rights reserved.
// </copyright>
// <summary>
//   Controller class user to control the Project Data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Projektverwaltung.Database;

namespace Projektverwaltung.Controllers
{
    /// <summary> The ProjekteController controlls the project Data for the application and consists of different Actions and methods to do so.</summary>
    public class ProjekteController : Controller
    {
        //Database context object
        private ProjektverwaltungEntities db = new ProjektverwaltungEntities();

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>List of all Projects</returns>
        /// Example: GET: Projekte
        public ActionResult Index()
        {
            //Create project object and include employee and vorgehensmodell data
            var projekt = db.Projekt.Include(p => p.Mitarbeiter).Include(p => p.Vorgehensmodell);

            //Populate Lists to ViewData
            ViewBag.StatusList = GetStatusList();
            ViewBag.PrioList = GetPrioList();

            //Return View
            return View(projekt.ToList());
        }

        /// <summary>
        /// Details Action for Projects
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>returns the detail View</returns>
        /// Example: GET: Projekte/Details/5
        public ActionResult Details(int? id)
        {
            //Check if an ID was set and if not, show a Bad-Request-Error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Search the employee in the database
            Projekt projekt = db.Projekt.Find(id);
            if (projekt == null)
            {
                return HttpNotFound();
            }

            //Populate Lists to ViewData
            ViewBag.StatusList = GetStatusList();
            ViewBag.PrioList = GetPrioList();

            //Return Details View
            return View(projekt);
        }

        /// <summary>
        /// Create Action for Projects
        /// </summary>
        /// <returns>returns the create view</returns>
        /// Example: GET: Projekte/Create
        public ActionResult Create()
        {
            //Populate Viewbag Data
            ViewBag.projektleiter_id = new SelectList(db.Mitarbeiter, "id", "name");
            ViewBag.vorgehensmodell_id = new SelectList(db.Vorgehensmodell.Where(s => s.status != 2) , "id", "name");
            ViewBag.StatusList = GetStatusList();
            ViewBag.PrioList = GetPrioList();

            //Return view
            return View();
        }

        /// <summary>
        /// Create Action that is called by a Post on the create view.
        /// Tries to save the changes posted back to the database
        /// </summary>
        /// <param name="projekt">Project Object</param>
        /// <returns>Create view with changed model</returns>
        /// Example POST: Projekte/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,beschreibung,status,prioritaet,startdatum_geplant,enddatum_geplant,startdatum_effektiv,enddatum_effektiv,fortschritt,dokument_link,projektleiter_id,vorgehensmodell_id")] Projekt projekt)
        {
            //Check if the Model (Data) is still valid
            if (ModelState.IsValid)
            {
                //Loop through all VorgehensmodellPhases
               foreach(VorgehensmodellPhase vp in db.VorgehensmodellPhase.Where(s => s.vorgehensmodell_id == projekt.vorgehensmodell_id))
                {
                    //Create and add a copy of the VorgehensModellPhases as ProjectPhase to the context
                    db.ProjektPhase.Add(new ProjektPhase()
                    {
                        name = vp.name,
                        beschreibung = vp.beschreibung,
                        projekt_id = projekt.id,
                        status = 1,
                        fortschritt = 0
                    });
                }
                //Add the project object to the context
                db.Projekt.Add(projekt);

                //Save the changes on the database (IDs for ProjektPhasen are now generated!)
                db.SaveChanges();

                //Loop through all ProjektPhasen (including generated IDs)
                foreach(ProjektPhase p in db.ProjektPhase.Where(p => p.projekt_id == projekt.id))
                {
                    //Add a new Activity with some default data where needed
                    db.Aktivitaet.Add(new Aktivitaet()
                    {
                        name = "Neue Aktivität",
                        projektphase_id = p.id,
                        status = 1,
                        fortschritt = 0
                    });
                    
                    //Add a new end-phase Milestone with default data where needed
                    db.Meilenstein.Add(new Meilenstein()
                    {
                        name = "Abschluss " + p.name + " Meilenstein",
                        status = 1,
                        nicht_loeschbar = 1,
                        projektphase_id = p.id
                    });
                }

                //Save the changes on the database
                db.SaveChanges();

                //Return to the index view
                return RedirectToAction("Index");
            }

            //Populate Viewbag Data for the view in case the creation and the redirect failed
            ViewBag.projektleiter_id = new SelectList(db.Mitarbeiter, "id", "name", projekt.projektleiter_id);
            ViewBag.vorgehensmodell_id = new SelectList(db.Vorgehensmodell, "id", "name", projekt.vorgehensmodell_id);

            //Return Create View in case the creation of the project failed
            return View(projekt);
        }

        /// <summary>
        /// Edit Action for Projects
        /// </summary>
        /// <param name="id"></param>
        /// <returns>edit view</returns>
        /// Example: GET: Projekte/Edit/5
        public ActionResult Edit(int? id)
        {
            //Check if an ID was set and if not, show a Bad-Request-Error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Search the project in the database
            Projekt projekt = db.Projekt.Find(id);
            if (projekt == null)
            {
                return HttpNotFound();
            }

            //Get state list for manipulations
            var statusList = GetStatusList();

            //Check if there are uncompleted project phases
            //If all are completed, enable project state "Abgeschlossen"
            if (db.ProjektPhase.Where(s => s.status != 4 && s.projekt_id == projekt.id).Count() == 0)
            {
                statusList.FirstOrDefault(x => x.Text == "Abgeschlossen").Disabled = false;
            }
            //NOTE: Activity states are checked within the project phase partials/templates!
            
            //Populate Viewbag Data for the Edit View
            ViewBag.StatusList = statusList;
            ViewBag.PhasenStatusList = GetPhasenStatusList();
            ViewBag.PrioList = GetPrioList();
            ViewBag.AktivitaetenStatusList = GetAktivitaetenStatusList();
            ViewBag.MeilensteinStatusList = GetMeilensteinStatusList();
            ViewBag.kostenart_id = new SelectList(db.Kostenart, "id", "name");
            ViewBag.mitarbeiter_id = new SelectList(db.Mitarbeiter, "id", "name");
            ViewBag.projektleiter_id = new SelectList(db.Mitarbeiter, "id", "name", projekt.projektleiter_id);
            ViewBag.vorgehensmodell_id = new SelectList(db.Vorgehensmodell, "id", "name", projekt.vorgehensmodell_id);
            return View(projekt);
        }

        /// <summary>
        /// Edit Action that is called by a Post on the edit view.
        /// Tries to save the changes posted back to the database
        /// </summary>
        /// <param name="projekt"></param>
        /// <returns>Edit view with changed model</returns>
        /// Example: POST: Projekte/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Projekt projekt)
        {
            //Check if the model is still valid
            if (ModelState.IsValid)
            {
                //Loop through the phases and update the entries in the db table
                foreach (var phase in projekt.ProjektPhase)
                {
                    var dbphase = db.ProjektPhase.FirstOrDefault(p => p.id == phase.id);
                    dbphase.name = phase.name;
                    dbphase.beschreibung = phase.beschreibung;
                    dbphase.status = phase.status;
                    dbphase.startdatum_geplant = phase.startdatum_geplant;
                    dbphase.enddatum_geplant = phase.enddatum_geplant;
                    dbphase.startdatum_effektiv = phase.startdatum_effektiv;
                    dbphase.enddatum_effektiv = phase.enddatum_effektiv;
                    dbphase.fortschritt = phase.fortschritt;
                    dbphase.freigabe_datum = phase.freigabe_datum;
                    dbphase.freigabe_visum = phase.freigabe_visum;
                    dbphase.dokumente_link = phase.dokumente_link;
                    dbphase.reviewdatum_geplant = phase.reviewdatum_geplant;
                    dbphase.reviewdatum_effektiv = phase.reviewdatum_effektiv;


                    //Loop through the activities and update the entries in the db table
                    foreach(var activity in phase.Aktivitaet)
                    {
                        var dbactivity = db.Aktivitaet.FirstOrDefault(a => a.id == activity.id);
                        dbactivity.name = activity.name;
                        dbactivity.status = activity.status;
                        dbactivity.startdatum_geplant = activity.startdatum_geplant;
                        dbactivity.enddatum_geplant = activity.enddatum_geplant;
                        dbactivity.startdatum_effektiv = activity.startdatum_effektiv;
                        dbactivity.enddatum_effektiv = activity.enddatum_effektiv;
                        dbactivity.erwartete_kosten = activity.erwartete_kosten;
                        dbactivity.effektive_kosten = activity.effektive_kosten;
                        dbactivity.fortschritt = activity.fortschritt;
                        dbactivity.dokumente_link = activity.dokumente_link;

                        //Loop through the tasks (AktivitaetMitarbeiter) and update the entries in the db table
                        foreach (var task in activity.AktivitaetMitarbeiter)
                        {
                            var dbtask = db.AktivitaetMitarbeiter.FirstOrDefault(a => a.id == task.id);
                            dbtask.funktion = task.funktion;
                            dbtask.kostenart_id = task.kostenart_id;
                            dbtask.mitarbeiter_id = task.mitarbeiter_id;
                            dbtask.budgetierte_zeit = task.budgetierte_zeit;
                            dbtask.effektive_zeit = task.effektive_zeit;
                            dbtask.abweichungsgrund = task.abweichungsgrund;
                        }

                        //Clear the navigation properties on the model so we don't get conflicts here
                        activity.AktivitaetMitarbeiter.Clear();
                    }

                    //Loop through the milestones and update the entries in the db table
                    foreach (var milestone in phase.Meilenstein)
                    {
                        var dbmilestone = db.Meilenstein.FirstOrDefault(m => m.id == milestone.id);
                        dbmilestone.name = milestone.name;
                        dbmilestone.status = milestone.status;
                        dbmilestone.beschreibung = milestone.beschreibung;
                        dbmilestone.nicht_loeschbar = milestone.nicht_loeschbar;
                    }


                    //Clear the navigation properties on the model so we don't get conflicts here
                    phase.Meilenstein.Clear();
                    phase.Aktivitaet.Clear();
                }

                //Clear the navigation properties on the model so we don't get conflicts here
                projekt.ProjektPhase.Clear();


                try
                {
                    //Update vorgehensmodell table and save changes
                    db.Entry(projekt).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    //Throw Exception
                    throw e;
                }

                //Redirect to Index View
                return RedirectToAction("Index");
            }

            //Repopulate the Viewbag Data in case the Changes Failed
            ViewBag.StatusList = GetStatusList();
            ViewBag.PhasenStatusList = GetPhasenStatusList();
            ViewBag.PrioList = GetPrioList();
            ViewBag.AktivitaetenStatusList = GetAktivitaetenStatusList();
            ViewBag.MeilensteinStatusList = GetMeilensteinStatusList();
            ViewBag.kostenart_id = new SelectList(db.Kostenart, "id", "name");
            ViewBag.projektleiter_id = new SelectList(db.Mitarbeiter, "id", "name", projekt.projektleiter_id);
            ViewBag.vorgehensmodell_id = new SelectList(db.Vorgehensmodell, "id", "name", projekt.vorgehensmodell_id);

            //Return the Edit View (Changes Failed)
            return View(projekt);
        }


        /// <summary>
        /// Action that adds activities to a given phase
        /// </summary>
        /// <param name="phaseId">ID of the Phase</param>
        /// <param name="projektId">ID of the Project</param>
        /// <returns>Redirect to Edit View</returns>
        public ActionResult AddAktivitaet(int phaseId, int projektId)
        {
            //Add new Activity
            db.Aktivitaet.Add(new Aktivitaet()
            {
                name = "Neue Aktivität",
                projektphase_id = phaseId,
                status = 1
            });

            //Try to save changes or throw exception
            try
            {
                db.SaveChanges();
            } catch (Exception e) { throw e; }

            //Redirect to the Edit Project View
            return this.RedirectToAction("Edit", new { id = projektId });
        }

        /// <summary>
        /// Action that removes activities from phase
        /// </summary>
        /// <param name="aktivitaetId">ID of the Activity</param>
        /// <param name="projektId">ID of the Project</param>
        /// <returns>Redirect to Edit View</returns>
        public ActionResult RemoveAktivitaet(int aktivitaetId, int projektId)
        {
            //Search Activity object in the database
            var akt = db.Aktivitaet.FirstOrDefault(a => a.id == aktivitaetId);

            //Remove activity from the context
            db.Aktivitaet.Remove(akt);

            //Try to save changes or throw exception
            try
            {
                db.SaveChanges();
            } catch (Exception e) { throw e;  }

            //Return to Edit Project View
            return this.RedirectToAction("Edit", new { id = projektId });
        }

        /// <summary>
        /// Action that adds milestones to a given phase
        /// </summary>
        /// <param name="phaseId">ID of the Phase</param>
        /// <param name="projektId">ID of the Project</param>
        /// <returns>Redirect to Edit View</returns>
        public ActionResult AddMeilenstein(int phaseId, int projektId)
        {
            //Add new Milestone
            db.Meilenstein.Add(new Meilenstein()
            {
                name = "Neuer Meilenstein",
                projektphase_id = phaseId,
                status = 1
            });
            //Try to save changes or throw exception
            try
            {
                db.SaveChanges();
            } catch (Exception e){ throw e; }

            //Return to the Edit Project View
            return this.RedirectToAction("Edit", new { id = projektId });
        }

        /// <summary>
        /// Action that removes milestones from phase
        /// </summary>
        /// <param name="meilensteinId">ID of the Milestone</param>
        /// <param name="projektId">ID of the Project</param>
        /// <returns>Redirect to Edit View</returns>
        public ActionResult RemoveMeilenstein(int meilensteinId, int projektId)
        {
            //Search milestone object in the database
            var me = db.Meilenstein.FirstOrDefault(a => a.id == meilensteinId);

            //Remove milestone from the context
            db.Meilenstein.Remove(me);

            //Try to save changes or throw exception
            try
            {
                db.SaveChanges();
            } catch (Exception e) { throw e; }

            //Return to Edit Project View
            return this.RedirectToAction("Edit", new { id = projektId });
        }

        /// <summary>
        /// Action that adds tasks to a given activity
        /// </summary>
        /// <param name="aktivitaetId">ID of the Activity</param>
        /// <param name="projektId">ID of the Project</param>
        /// <returns>Redirect to Edit View</returns>
        public ActionResult AddTask(int aktivitaetId, int projektId)
        {
            //Add new Task (AktivitaetMitarbeiter)
            db.AktivitaetMitarbeiter.Add(new AktivitaetMitarbeiter()
            {
                funktion = "Neuer Task",
                aktivitaet_id = aktivitaetId,
                kostenart_id = 1,
                mitarbeiter_id = 1
            });

            //Try to save the changes or throw exception
            try
            {
                db.SaveChanges();
            }
            catch (Exception e) { throw e; }

            //Return to Edit Project View
            return this.RedirectToAction("Edit", new { id = projektId });
        }

        /// <summary>
        /// Action that removes tasks from an activity
        /// </summary>
        /// <param name="taskId">ID of the Task</param>
        /// <param name="projektId">ID of the Project</param>
        /// <returns>Redirect to Edit View</returns>
        public ActionResult RemoveTask(int taskId, int projektId)
        {
            //Find Task (AktivitaetMitarbeiter) in the database
            var task = db.AktivitaetMitarbeiter.FirstOrDefault(t => t.id == taskId);

            //Remove task from the context
            db.AktivitaetMitarbeiter.Remove(task);

            //Try to save changes or throw exception
            try
            {
                db.SaveChanges();
            }
            catch (Exception e) { throw e; }

            //Return to Edit Project View
            return this.RedirectToAction("Edit", new { id = projektId });
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

        /// <summary>
        /// List to Populate Project states
        /// </summary>
        /// <returns>SelectList with all Project states</returns>
        private IEnumerable<SelectListItem> GetStatusList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "In Planung", Disabled = false });
            list.Add(new SelectListItem { Value = "2", Text = "Geplant", Disabled = false });
            list.Add(new SelectListItem { Value = "3", Text = "Bewilligt", Disabled = false });
            list.Add(new SelectListItem { Value = "4", Text = "In Arbeit", Disabled = false });
            list.Add(new SelectListItem { Value = "5", Text = "Abgeschlossen", Disabled = true });
            return list;
        }

        /// <summary>
        /// List to Populate Phase states
        /// </summary>
        /// <returns>SelectList with all Phase states</returns>
        private IEnumerable<SelectListItem> GetPhasenStatusList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "In Planung", Disabled = false });
            list.Add(new SelectListItem { Value = "2", Text = "Freigegeben", Disabled = false });
            list.Add(new SelectListItem { Value = "3", Text = "In Arbeit", Disabled = false });
            list.Add(new SelectListItem { Value = "4", Text = "Abgeschlossen", Disabled = true });
            return list;
        }

        /// <summary>
        /// List to Populate Activity states
        /// </summary>
        /// <returns>SelectList with all Activity states</returns>
        private IEnumerable<SelectListItem> GetAktivitaetenStatusList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "In Planung", Disabled = false });
            list.Add(new SelectListItem { Value = "2", Text = "In Arbeit", Disabled = false });
            list.Add(new SelectListItem { Value = "3", Text = "Abgeschlossen", Disabled = false });
            return list;
        }

        /// <summary>
        /// List to Populate Project priorities
        /// </summary>
        /// <returns>SelectList with all project priorities</returns>
        private IEnumerable<SelectListItem> GetPrioList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "1 - Tief", Disabled = false });
            list.Add(new SelectListItem { Value = "2", Text = "2 - Mittel", Disabled = false });
            list.Add(new SelectListItem { Value = "3", Text = "3 - Hoch", Disabled = false });
            return list;
        }

        /// <summary>
        /// List to Populate Milestone states
        /// </summary>
        /// <returns>SelectList with all Milestone states</returns>
        private IEnumerable<SelectListItem> GetMeilensteinStatusList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "Nicht erreicht", Disabled = false });
            list.Add(new SelectListItem { Value = "2", Text = "Erreicht", Disabled = false });
            return list;
        }

    }
}
