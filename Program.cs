using System;
using System.Collections.Generic;
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
            //InserirDadosEmMassa();
            // ConsultarDados();
            //CadastrarDados();

            //ConsultarPedidoCarregamentoAdiantado();

            //AtualizarDados();

            DeletarDados();

            using var db = new AppDbContext();
            
            //Verificar se há validações pendentes
            var existe = db.Database.GetPendingMigrations().Any() ;
            if (existe)
            {
                Console.WriteLine("Existem Migrações pendentes");
            }
        }

        private static void DeletarDados() 
        {
            using var db = new AppDbContext();

            var cliente = db.Clientes.Find(2); //utiliza chave primária da entidade

            //db.Clientes.Remove(cliente);
            //db.Remove(cliente);
            //db.Entry(cliente).State = EntityState.Deleted;

            //Remoção de dados desconectados - diferença de localização do dado antes da remoção (apenas deleta)

            var clienteDesconectado = new Cliente()
            {
                Id = 3
            };

            db.Remove(clienteDesconectado);

            db.SaveChanges();

        }

        private static void AtualizarDados() 
        {
            using var db = new AppDbContext();

            //var cliente = db.Clientes.FirstOrDefault(p => p.Id == 1); // ou Find(1) => 1° cliente

            //cliente.Nome = "Anna";
            //db.Entry(cliente).State = EntityState.Modified;//forçando o rastreamento da entidade
            //db.Update(cliente.Nome);
            //db.SaveChanges();

            var cliente = new Cliente()
            {
                Id = 1
            };

            //Atualizando os dados anônimos - alterando apenas campos que sofreram alterações
            var clienteDesconectado = new 
            {
                Nome = "Cliente desconetado Teste 3",
                Telefone = "55223232731"
            };

            db.Attach(cliente); // o objeto não está sendo rastreado pelo EF Core. o attach faz um rastreio interno
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

        }

        private static void ConsultarPedidoCarregamentoAdiantado() 
        {
            using var db = new AppDbContext();

            var pedidos = db.Pedidos.ToList(); //carregamento apenas do pedido
            var pedidos2 = db.Pedidos
                .Include(p => p.Items)
                .ThenInclude(p => p.Produto)
                .ToList(); //carregamento apenas do pedido e pedidoItens usando lambda

            Console.WriteLine($"Quantidade de pedidos {pedidos.Count}");
            Console.WriteLine($"Quantidade de pedidos {pedidos2.Count}");
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

        private static void CadastrarDados() 
        {
            using var db = new AppDbContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido()
            {
                ClienteId = cliente.Id,
                CriadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                TipoFrete = TipoFrete.CIF,
                Status = StatusPedido.Analise,
                Observacao = "Observacao",
                Items = new List<PedidoItem>
                {
                     new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Quantidade = 2,
                        Valor = 10,
                        Desconto = 0
                    }
                }
            };

            db.Add(pedido);

            db.SaveChanges();
        

    }


        /// <summary>
        /// Todos os métodos fazem a consulta pela base de dados 
        /// com excessão do método Find(), que prioriza a busca por memória
        /// 
        /// O EF Core tem suporte para métodos linq
        /// </summary>
        private static void ConsultarDados() 
        {
            using var db = new AppDbContext();

            //var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList(); //consulta por sintaxe
            //var consultaPorMetodo = db.Clientes.Where(p => p.Id > 0).ToList();//consulta por método usando labda
            var consultaPorMetodo = db.Clientes.AsNoTracking()
                .Where(p => p.Id > 0)
                .OrderBy(p => p.Id)
                .ToList(); //busca apenasa na base de dados AsNoTracking

            foreach (var cliente in consultaPorMetodo) 
            {
                Console.WriteLine($"Consultando cliente: { cliente.Id}");
                //db.Clientes.Find(cliente.Id); //consulta feita na chave primária - busca primeiro em memória e depois na base
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }
    }
}
