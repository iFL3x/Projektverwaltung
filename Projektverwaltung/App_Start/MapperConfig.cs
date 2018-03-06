using AutoMapper;
using Projektverwaltung.Database;
using Projektverwaltung.Models;
using System.Collections.Generic;
using System.Linq;

namespace Projektverwaltung
{
    public class MapperConfig
    {
        private readonly ProjektverwaltungEntities db;

        public MapperConfig()
        {
            this.db = new ProjektverwaltungEntities();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Mitarbeiter, MitarbeiterViewModel>()
                    .ForMember(d => d.Funktionen, o => o.MapFrom(s => this.GetEmployeeFunctions(s.id)));
                cfg.CreateMap<MitarbeiterViewModel, Mitarbeiter>();

                cfg.CreateMap<Vorgehensmodell, VorgehensmodellViewModel>()
                    .ForMember(d => d.Phasen, o => o.MapFrom(s => this.GetVorgehensmodellPhasen(s.id)));
                cfg.CreateMap<VorgehensmodellViewModel, Vorgehensmodell>();
            });
        }


        // this kind of functions shoud be moved to your database repository project
        private IEnumerable<Funktion> GetEmployeeFunctions(int employeeId)
        {
            foreach (var item in this.db.MitarbeiterFunktion.Where(x => x.mitarbeiter_id == employeeId))
            {
                yield return item.Funktion;
            }
        }

        private IEnumerable<VorgehensmodellPhase> GetVorgehensmodellPhasen(int vorgehensmodellId)
        {
            foreach (var item in this.db.VorgehensmodellPhase.Where(x => x.vorgehensmodell_id == vorgehensmodellId))
            {
                yield return item;
            }
        }
    }
}
