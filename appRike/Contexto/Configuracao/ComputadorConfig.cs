using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Configuracao
{
    public class ComputadorConfig : EntityTypeConfiguration<Computador>
    {
        public ComputadorConfig()
        {
            ToTable("Computadores");
            HasKey(e => new { e.Id });

            Property(e => e.Id)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
             .IsRequired();

            Property(e => e.Descricao)
                .IsRequired();
        }
    }
}
