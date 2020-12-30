using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Curso.Data.Configurations
{
    class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CriadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd(); //insere um valor default
            builder.Property(p => p.Status).HasConversion<string>();
            builder.Property(p => p.TipoFrete).HasConversion<int>(); //conversao do enum para int
            builder.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

            //relação de 1 para muitos
            builder.HasMany(p => p.Items)
            .WithOne(p => p.Pedido) //1 pedido tem vários itens
            .OnDelete(DeleteBehavior.Cascade); //se deletar 1 pedido, deleta todos os itens no pedido (cascade)
        }
    }
}
