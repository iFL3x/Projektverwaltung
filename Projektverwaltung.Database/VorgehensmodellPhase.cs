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
    
    public partial class VorgehensmodellPhase
    {
        public int id { get; set; }
        public string name { get; set; }
        public string beschreibung { get; set; }
        public int vorgehensmodell_id { get; set; }
    
        public virtual Vorgehensmodell Vorgehensmodell { get; set; }
    }
}
