using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Curso.Data.Configurations
{
    class PedidoItemConfiguration : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("PedidoItens");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Quantidade).HasDefaultValue(1).IsRequired(); //a quantidade do item tem o valor 1 como default
            builder.Property(p => p.Valor).IsRequired();
            builder.Property(p => p.Desconto).IsRequired();
        }
    }
}
