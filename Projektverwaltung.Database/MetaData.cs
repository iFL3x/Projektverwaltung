using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektverwaltung.Database
{

    [MetadataType(typeof(MitarbeiterMetaData))]
    public partial class Mitarbeiter
    {
        public List<Funktion> funktionen { get; set; }
    }
    public partial class MitarbeiterMetaData
    {
        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("Name")]
        public string name { get; set; }
        [DisplayName("Vorname")]
        public string vorname { get; set; }
        [DisplayName("Abteilung")]
        public string abteilung { get; set; }
        [DisplayName("Pensum")]
        [Range(1, 100)]
        public int pensum { get; set; }
    }



    [MetadataType(typeof(VorgehensmodellMetaData))]
    public partial class Vorgehensmodell
    {
    }
    public partial class VorgehensmodellMetaData
    {
        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("Name")]
        public string name { get; set; }
        [Range(1, 2)]
        [DisplayName("Status")]
        public int status { get; set; }
    }


    [MetadataType(typeof(ProjektMetaData))]
    public partial class Projekt
    {
    }
    public partial class ProjektMetaData
    {
        [DisplayName("ID")]
        public int id { get; set; }

        [DisplayName("Name")]
        public string name { get; set; }

        [DisplayName("Beschreibung")]
        [DataType(DataType.MultilineText)]
        public string beschreibung { get; set; }

        [DisplayName("Status")]
        [Range(1, 5)]
        public string status { get; set; }

        [DisplayName("Priorität")]
        [Range(1, 3)]
        public int prioritaet { get; set; }

        [DisplayName("Startdatum gelant")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime startdatum_geplant { get; set; }

        [DisplayName("Enddatum geplant")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime enddatum_geplant { get; set; }

        [DisplayName("Startdatum effektiv")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime startdatum_effektiv { get; set; }

        [DisplayName("Enddatum effektiv")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime enddatum_effektiv { get; set; }

        [DisplayName("Bewilligungsdatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime bewilligungsdatum { get; set; }

        [DisplayName("Fortschritt")]
        [Range(0, 100)]
        [Required]
        public Nullable<double> fortschritt { get; set; }

        [DisplayName("Dokumente")]
        public string dokument_link { get; set; }

        [DisplayName("Projektleiter")]
        public int projektleiter_id { get; set; }

        [DisplayName("Vorgehensmodell")]
        [Required]
        public int vorgehensmodell_id { get; set; }
    }



    [MetadataType(typeof(VorgehensmodellPhaseMetaData))]
    public partial class VorgehensmodellPhase
    {
    }
    public partial class VorgehensmodellPhaseMetaData
    {
        [DisplayName("ID")]
        public int id { get; set; }

        [DisplayName("Name")]
        public string name { get; set; }

        [DisplayName("Beschreibung")]
        [DataType(DataType.MultilineText)]
        public string beschreibung { get; set; }

        [DisplayName("Vorgehensmodell")]
        public int vorgehensmodell_id { get; set; }
    }


    [MetadataType(typeof(ProjektPhaseMetaData))]
    public partial class ProjektPhase
    {
    }

    public partial class ProjektPhaseMetaData
    {
        [DisplayName("ID")]
        public int id { get; set; }

        [DisplayName("Name")]
        [Required]
        public string name { get; set; }

        [DisplayName("Beschreibung")]
        [DataType(DataType.MultilineText)]
        public string beschreibung { get; set; }

        [DisplayName("Status")]
        [Required]
        public string status { get; set; }

        [DisplayName("Startdatum geplant")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> startdatum_geplant { get; set; }

        [DisplayName("Enddatum geplant")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> enddatum_geplant { get; set; }

        [DisplayName("Startdatum effektiv")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> startdatum_effektiv { get; set; }

        [DisplayName("Enddatum effektiv")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> enddatum_effektiv { get; set; }

        [DisplayName("Forschritt")]
        [Range(0, 100)]
        public Nullable<double> fortschritt { get; set; }

        [DisplayName("Freigabe Datum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> freigabe_datum { get; set; }

        [DisplayName("Freigabe Visum")]
        public string freigabe_visum { get; set; }

        [DisplayName("Dokumente")]
        public string dokumente_link { get; set; }

        [DisplayName("Review Datum geplant")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> reviewdatum_geplant { get; set; }

        [DisplayName("Review Datum effektiv")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> reviewdatum_effektiv { get; set; }

        [DisplayName("Projekt")]
        public int projekt_id { get; set; }
    }

    [MetadataType(typeof(AktivitaetMetaData))]
    public partial class Aktivitaet
    {
    }

    public partial class AktivitaetMetaData
    {
        [DisplayName("ID")]
        public int id { get; set; }

        [DisplayName("Name")]
        public string name { get; set; }

        [DisplayName("Status")]
        public Nullable<int> status { get; set; }

        [DisplayName("Projektphase")]
        public int projektphase_id { get; set; }

        [DisplayName("Startdatum geplant")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime startdatum_geplant { get; set; }

        [DisplayName("Enddatum geplant")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime enddatum_geplant { get; set; }

        [DisplayName("Startdatum effektiv")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> startdatum_effektiv { get; set; }

        [DisplayName("Enddatum effektiv")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> enddatum_effektiv { get; set; }

        [DisplayName("Erwartete Externe Kosten")]
        [DataType(DataType.Currency)]
        public Nullable<double> erwartete_kosten { get; set; }

        [DisplayName("Effektive Externe Kosten")]
        [DataType(DataType.Currency)]
        public Nullable<double> effektive_kosten { get; set; }

        [DisplayName("Kostenart")]
        public int kostenart_id { get; set; }

        [DisplayName("Fortschritt")]
        public Nullable<double> fortschritt { get; set; }

        [DisplayName("Dokumente")]
        public string dokumente_link { get; set; }
    }
}