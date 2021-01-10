using Curso.Data.Configurations;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


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

            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            //modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            //modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());
            //modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
        }
    }
}
