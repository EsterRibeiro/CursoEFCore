using System;
using System.Linq;
using Curso.Data;
using Curso.Domain;
using Curso.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Curso
{
    class Program
    {
        static void Main(string[] args)
        {
            InserirDados();

            using var db = new Data.AppDbContext();
            
            //Verificar se há validações pendentes
            var existe = db.Database.GetPendingMigrations().Any() ;
            if (existe)
            {
                Console.WriteLine("Existem Migrações pendentes");
            }
        }

        private static void InserirDados() 
        {
            //todas as interações do banco são através do context
            var produto = new Produto
            {
                Descricao = "Sabonete",
                CodigoBarras = "123456788897",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            //possibilidades de inserção (rastreado pelo EF Core)

            //instancia do contexto
            using var db = new AppDbContext();

            db.Produtos.Add(produto);
            //db.Produtos.AddRange(produto); //lista de produtos
            db.Set<Produto>().Add(produto);//usando método genérico
            db.Entry(produto).State = EntityState.Added;//forçando rastreamento de uma entidade
            db.Add(produto);//própria instância do contexto

            var registro = db.SaveChanges(); //tudo que foi rastreado (em memória), gera instruções no banco
            Console.WriteLine(registro);
        }
    }
}
