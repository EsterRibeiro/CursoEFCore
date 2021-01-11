using Curso.Data.Configurations;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Curso.Data
{
    public class AppDbContext : DbContext
    {
        //instância do logger
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos{ get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        //Método de configuração da string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseLoggerFactory(_logger)//qual log estou usando
            .EnableSensitiveDataLogging() //exibe os valores dos parâmetros gerados pelo EF Core
            .UseSqlite("Data Source=C:\\Users\\ester.santos\\Desktop\\SQLiteStudio\\CursoEFCore.db;");
        }

        //Especificando a entidade
        //Por FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            MapearPropriedadesEsquecidas(modelBuilder);

            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            //modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            //modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());
            //modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
        }

        //Configuração automática de propriedades

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder) 
        {
            //foreach de todas as entidades
            foreach(var entity in modelBuilder.Model.GetEntityTypes()) 
            {
                //pegando todas as propriedadesc do tipo string
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach(var property in properties) 
                {
                    if(string.IsNullOrEmpty(property.GetColumnType()) && !property.GetMaxLength().HasValue)
                    {
                        //property.SetMaxLength(100);
                        property.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }
    }
}
