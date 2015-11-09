using Dominio.Entidades;
using Infra.Configuracao;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Infra.Contexto
{    public class DbContexto : DbContext
    {
        public DbContexto()
        : base("BancoRike"){ }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Computador> Computadores { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Agenda> Agendas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(200));
            modelBuilder.Properties<DateTime>().Configure(p => p.HasColumnType("datetime"));

            modelBuilder.Configurations.Add(new AlunoConfig());
            modelBuilder.Configurations.Add(new ComputadorConfig());
            modelBuilder.Configurations.Add(new HorarioConfig());
            modelBuilder.Configurations.Add(new AgendaConfig());
        }
    }
}


