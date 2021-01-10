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
            //InserirDados();
            InserirDadosEmMassa();
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


        private static void InserirDadosEmMassa()
        {
            var produto = new Produto()
            {
                Descricao = "Esponja",
                CodigoBarras = "1234567244442",
                Valor = 18m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente() 
            {
                Nome = "Ester",
                Telefone = "21 2683263236",
                CEP = "2287232",
                Estado = "RJ",
                Cidade = "RJ"
            };

            var listaClientes = new[]
            {
                new Cliente()
                {
                    Nome = "Maria",
                    Telefone = "21 3322323",
                    CEP = "2287232",
                    Estado = "ES",
                    Cidade = "Vitótia"

                },

                new Cliente()
                {
                    Nome = "Joana",
                    Telefone = "21 34522232",
                    CEP = "2287232",
                    Estado = "SP",
                    Cidade = "SP"

                }

            };

            using var db = new AppDbContext();
            //db.AddRange(produto, cliente);
            //db.AddRange(listaClientes);
            db.Set<Cliente>().AddRange(listaClientes);
            var registros = db.SaveChanges();
            Console.WriteLine(registros);
        
        }
    }
}
