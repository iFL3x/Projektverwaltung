// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" author="Fabrice Koenig">
//   Copyright (c) Fabrice Koenig Ltd. All rights reserved.
// </copyright>
// <summary>
// Class to create mappings. Uses AutoMapper to automatically Map Source to Destination Classes/Viewmodels in the controllers
// later
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using AutoMapper;
using Projektverwaltung.Database;
using Projektverwaltung.Models;
using System.Collections.Generic;
using System.Linq;

namespace Projektverwaltung
{
    public class MapperConfig
    {
        //Readonly database context used for automapping
        private readonly ProjektverwaltungEntities db;

        public MapperConfig()
        {
            this.db = new ProjektverwaltungEntities();

            //Mapper Config for Automapping
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Mitarbeiter, MitarbeiterViewModel>()
                    .ForMember(d => d.Funktionen, o => o.MapFrom(s => this.GetEmployeeFunctions(s.id)));
                cfg.CreateMap<MitarbeiterViewModel, Mitarbeiter>();
            });
        }


        /// <summary>
        /// Get all functions connected to a specific employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        /// TODO: This function should be moved to the database project
        private IEnumerable<Funktion> GetEmployeeFunctions(int employeeId)
        {
            foreach (var item in this.db.MitarbeiterFunktion.Where(x => x.mitarbeiter_id == employeeId))
            {
                yield return item.Funktion;
            }
        }
    }
}
