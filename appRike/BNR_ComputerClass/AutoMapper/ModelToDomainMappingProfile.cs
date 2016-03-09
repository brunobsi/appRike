using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BNR_ComputerClass.Models;
using Dominio.Entidades;

namespace BNR_ComputerClass.AutoMapper
{
    public class ModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<HorarioModel, Horario>();
            Mapper.CreateMap<ComputadorModel, Computador>();
            Mapper.CreateMap<AlunoModel, Aluno>();
            Mapper.CreateMap<AgendaModel, Agenda>();
        }
    }
}