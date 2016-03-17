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
            Mapper.CreateMap<Horario, HorarioModel>()
                .ForMember(p => p.HorarioSelect, opt => opt.MapFrom(src =>
                     string.Format("{0} das {1} às {2}", src.Dia, src.HoraInicial, src.HoraFinal)
                    ));

            Mapper.CreateMap<Computador, ComputadorModel>();
            Mapper.CreateMap<Agenda, AgendaModel>();
            Mapper.CreateMap<Aluno, AlunoModel>();
            Mapper.CreateMap<Aula, AulaModel>();
            Mapper.CreateMap<Chamada, ChamadaModel>();
        }
    }
}