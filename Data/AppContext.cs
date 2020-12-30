using Curso.Data.Configurations;
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);

            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            //modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            //modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());
            //modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
        }
    }
}
