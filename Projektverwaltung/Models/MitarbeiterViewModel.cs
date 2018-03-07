// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MitarbeiterViewModel.cs" author="Fabrice Koenig">
//   Copyright (c) Fabrice Koenig Ltd. All rights reserved.
// </copyright>
// <summary>
//   ViewModel Class for the Employee Data. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using Projektverwaltung.Database;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Projektverwaltung.Models
{
    /// <summary>
    /// The MitarbeiterViewModel class is used as a ViewModel for the EmployeeData inside the MitarbeiterController
    /// </summary>
    public class MitarbeiterViewModel
    {

        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Vorname")]
        public string Vorname { get; set; }
        
        [DisplayName("Abteilung")]
        public string Abteilung { get; set; }

        [DisplayName("Pensum")]
        [Range(1, 100)]
        public int Pensum { get; set; }

        [DisplayName("Funktionen")]
        public IEnumerable<Funktion> Funktionen { get; set; }
    }
}