﻿using Dominio.Entidades;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Infra.Configuracao
{
    public class HorarioConfig : EntityTypeConfiguration<Horario>
    {
        public HorarioConfig()
        {
            ToTable("Horarios");
            HasKey(e => new { e.Id });

            Property(e => e.Id)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
             .IsRequired();
        }
    }
}
