
using Projektverwaltung.Database;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Projektverwaltung.Models
{
    public class MitarbeiterViewModel
    {
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Vorname { get; set; }
        
        public string Abteilung { get; set; }

        [DisplayName("Pensum")]
        [Range(1, 100)]
        public int Pensum { get; set; }

        public IEnumerable<Funktion> Funktionen { get; set; }
    }
}