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
    
    public partial class MitarbeiterFunktion
    {
        public int id { get; set; }
        public int mitarbeiter_id { get; set; }
        public int funktion_id { get; set; }
    
        public virtual Funktion Funktion { get; set; }
        public virtual Mitarbeiter Mitarbeiter { get; set; }
    }
}
