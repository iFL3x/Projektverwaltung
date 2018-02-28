//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projektverwaltung.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjektPhase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjektPhase()
        {
            this.Aktivitaet = new HashSet<Aktivitaet>();
            this.Dokument = new HashSet<Dokument>();
            this.Meilenstein = new HashSet<Meilenstein>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string beschreibung { get; set; }
        public string status { get; set; }
        public System.DateTime startdatum_geplant { get; set; }
        public System.DateTime enddatum_geplant { get; set; }
        public System.DateTime startdatum_effektiv { get; set; }
        public System.DateTime enddatum_effektiv { get; set; }
        public Nullable<double> fortschritt { get; set; }
        public Nullable<System.DateTime> freigabe_datum { get; set; }
        public string freigabe_visum { get; set; }
        public string dokumente_link { get; set; }
        public Nullable<System.DateTime> reviewdatum_geplant { get; set; }
        public Nullable<System.DateTime> reviewdatum_effektiv { get; set; }
        public int projekt_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Aktivitaet> Aktivitaet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dokument> Dokument { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Meilenstein> Meilenstein { get; set; }
        public virtual Projekt Projekt { get; set; }
    }
}
