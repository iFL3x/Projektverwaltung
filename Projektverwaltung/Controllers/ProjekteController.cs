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

        

        // GET: Projekte
        public ActionResult Index()
        {
            var projekt = db.Projekt.Include(p => p.Mitarbeiter).Include(p => p.Vorgehensmodell);
            ViewBag.StatusList = GetStatusList();
            ViewBag.PrioList = GetPrioList();
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

            ViewBag.StatusList = GetStatusList();
            ViewBag.PrioList = GetPrioList();

            return View(projekt);
        }

        // GET: Projekte/Create
        public ActionResult Create()
        {
            ViewBag.projektleiter_id = new SelectList(db.Mitarbeiter, "id", "name");
            ViewBag.vorgehensmodell_id = new SelectList(db.Vorgehensmodell.Where(s => s.status != 2) , "id", "name");

            ViewBag.StatusList = GetStatusList();
            ViewBag.PrioList = GetPrioList();

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

               foreach(VorgehensmodellPhase vp in db.VorgehensmodellPhase.Where(s => s.vorgehensmodell_id == projekt.vorgehensmodell_id))
                {
                    db.ProjektPhase.Add(new ProjektPhase()
                    {
                        name = vp.name,
                        beschreibung = vp.beschreibung,
                        projekt_id = projekt.id,
                        status = 1
                    });
                }

                db.Projekt.Add(projekt);
                db.SaveChanges();

                

                foreach(ProjektPhase p in db.ProjektPhase.Where(p => p.projekt_id == projekt.id))
                {

                    db.Aktivitaet.Add(new Aktivitaet()
                    {
                        name = "Neue Aktivität",
                        projektphase_id = p.id,
                        kostenart_id = 1,
                        status = 1
                    });

                    db.Meilenstein.Add(new Meilenstein()
                    {
                        name = p.name + " Meilenstein",
                        projektphase_id = p.id
                    });
                }

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
            //Get state list for manipulations
            var statusList = GetStatusList();

            //Check if there are uncompleted project phases
            //If all are completed, enable project state "Abgeschlossen"
            if (db.ProjektPhase.Where(s => s.status != 4 && s.projekt_id == projekt.id).Count() == 0)
            {
                statusList.FirstOrDefault(x => x.Text == "Abgeschlossen").Disabled = false;
            }
            //Activity states are checked within the project phase partials!
            

            ViewBag.StatusList = statusList;
            ViewBag.PhasenStatusList = GetPhasenStatusList();
            ViewBag.PrioList = GetPrioList();
            ViewBag.AktivitaetenStatusList = GetAktivitaetenStatusList();

            ViewBag.kostenart_id = new SelectList(db.Kostenart, "id", "name");
            ViewBag.projektleiter_id = new SelectList(db.Mitarbeiter, "id", "name", projekt.projektleiter_id);
            ViewBag.vorgehensmodell_id = new SelectList(db.Vorgehensmodell, "id", "name", projekt.vorgehensmodell_id);
            return View(projekt);
        }

        // POST: Projekte/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Projekt projekt)
        {

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
                        dbactivity.kostenart_id = activity.kostenart_id;
                        dbactivity.fortschritt = activity.fortschritt;
                        dbactivity.dokumente_link = activity.dokumente_link;
                    }
                    //Clear the table on the model so we don't get conflicts here
                    phase.Aktivitaet.Clear();
                }

                //Clear the table on the model so we don't get conflicts here
                projekt.ProjektPhase.Clear();


                try
                {
                    //Update vorgehensmodell table and save changes
                    db.Entry(projekt).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }

                return RedirectToAction("Index");
            }
            ViewBag.StatusList = GetStatusList();
            ViewBag.PhasenStatusList = GetPhasenStatusList();
            ViewBag.PrioList = GetPrioList();
            ViewBag.AktivitaetenStatusList = GetAktivitaetenStatusList();
            ViewBag.kostenart_id = new SelectList(db.Kostenart, "id", "name");
            ViewBag.projektleiter_id = new SelectList(db.Mitarbeiter, "id", "name", projekt.projektleiter_id);
            ViewBag.vorgehensmodell_id = new SelectList(db.Vorgehensmodell, "id", "name", projekt.vorgehensmodell_id);

            return View(projekt);
        }


        public ActionResult AddAktivitaet(int phaseId, int projektId)
        {

            db.Aktivitaet.Add(new Aktivitaet()
            {
                name = "Neue Aktivität",
                projektphase_id = phaseId,
                kostenart_id = 1,
                status = 1
            });

             try
             {
                 db.SaveChanges();
             }
             catch (Exception e)
             {
                throw e;
             }

            return this.RedirectToAction("Edit", new { id = projektId });
        }

        public ActionResult RemoveAktivitaet(int aktivitaetId, int projektId)
        {

            var akt = db.Aktivitaet.FirstOrDefault(a => a.id == aktivitaetId);
            db.Aktivitaet.Remove(akt);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            return this.RedirectToAction("Edit", new { id = projektId });
        }

















        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }






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

        private IEnumerable<SelectListItem> GetPhasenStatusList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "In Planung", Disabled = false });
            list.Add(new SelectListItem { Value = "2", Text = "Freigegeben", Disabled = false });
            list.Add(new SelectListItem { Value = "3", Text = "In Arbeit", Disabled = false });
            list.Add(new SelectListItem { Value = "4", Text = "Abgeschlossen", Disabled = true });
            return list;
        }

        private IEnumerable<SelectListItem> GetAktivitaetenStatusList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "In Planung", Disabled = false });
            list.Add(new SelectListItem { Value = "2", Text = "In Arbeit", Disabled = false });
            list.Add(new SelectListItem { Value = "3", Text = "Abgeschlossen", Disabled = false });
            return list;
        }

        private IEnumerable<SelectListItem> GetPrioList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "1 - Tief", Disabled = false });
            list.Add(new SelectListItem { Value = "2", Text = "2 - Mittel", Disabled = false });
            list.Add(new SelectListItem { Value = "3", Text = "3 - Hoch", Disabled = false });
            return list;
        }

    }
}
