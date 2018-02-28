using Projektverwaltung.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projektverwaltung.Models
{
    public class MitarbeiterViewModel
    {
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Vorname { get; set; }
        
        public string Abteilung { get; set; }
        
        public int Pensum { get; set; }

        public IEnumerable<Funktion> Funktionen { get; set; }
    }
}