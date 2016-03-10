using AutoMapper;
using BNR_ComputerClass.Models;
using Dominio.Entidades;

namespace BNR_ComputerClass.AutoMapper
{
    public class DomainToModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Horario, HorarioModel>();
            Mapper.CreateMap<Computador, ComputadorModel>();
            Mapper.CreateMap<Aluno, AlunoModel>();
            Mapper.CreateMap<Agenda, AgendaModel>();
            Mapper.CreateMap<Aula, AulaModel>();
            Mapper.CreateMap<Chamada, ChamadaModel>();
        }
    }
}