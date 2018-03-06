using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projektverwaltung.Models
{
    public class VorgehensmodellViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        public IEnumerable<Database.VorgehensmodellPhase> Phasen { get; set; }

    }
}