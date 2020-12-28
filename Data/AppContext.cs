using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Curso.Data
{
    public class AppContext : DbContext
    {
        public DbSet<Pedido> Pedidos { get; set; }

        //Método de configuração da string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlite("Data Source=c:\\mydb.db;Version=3");
        }

        //Especificando a entidade
        //Por FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //usando lambda
            modelBuilder.Entity<Cliente>(p =>
            {
                p.ToTable("Clientes");
                p.HasKey(p => p.Id);
                p.Property(p => p.Nome).HasColumnType("VARCHAR(80)").IsRequired();
                p.Property(p => p.Telefone).HasColumnType("CHAR(11)"); //=> char tamanho estático
                p.Property(p => p.CEP).HasColumnType("CHAR(8)").IsRequired();
                p.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
                p.Property(p => p.Cidade).HasMaxLength(60).IsRequired(); //tamanho máximo de caracters

                p.HasIndex(i => i.Telefone).HasName("idx_cliente_telefone"); //criando índice na base de dados
            });

            modelBuilder.Entity<Produto>(p =>
            {
                p.ToTable("Produtos");
                p.HasKey(p => p.Id);
                p.Property(p => p.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
                p.Property(p => p.Descricao).HasColumnType("VARCHAR(60)");
                p.Property(p => p.Valor).IsRequired();
                p.Property(p => p.TipoProduto).HasConversion<string>(); //sendo um enum, se quero armazenar como int ou string
            });

            modelBuilder.Entity<Pedido>(p => 
            {
                p.ToTable("Pedidos");
                p.HasKey(p => p.Id);
                p.Property(p => p.CriadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd(); //insere um valor default
                p.Property(p => p.Status).HasConversion<string>();
                p.Property(p => p.TipoFrete).HasConversion<int>(); //conversao do enum para int
                p.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

                //relação de 1 para muitos
                p.HasMany(p => p.Items)
                .WithOne(p => p.Pedido) //1 pedido tem vários itens
                .OnDelete(DeleteBehavior.Cascade); //se deletar 1 pedido, deleta todos os itens no pedido (cascade)
            });

            modelBuilder.Entity<PedidoItem>(p =>
            {
                p.ToTable("PedidoItens");
                p.HasKey(p => p.Id);
                p.Property(p => p.Quantidade).HasDefaultValue(1).IsRequired(); //a quantidade do item tem o valor 1 como default
                p.Property(p => p.Valor).IsRequired();
                p.Property(p => p.Desconto).IsRequired();
            });
        }
    }
}
