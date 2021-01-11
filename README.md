# CursoEFCore
Curso de Entity Framework Core básico da plataforma Desenvolvedor.IO

Material:


[Connection Strings](https://www.connectionstrings.com/ "Connection Strings Homepage")


[Comandos EF Core](https://www.entityframeworktutorial.net/efcore/entity-framework-core-migration.aspx "EF Tutorial")


[Anotações pessoais](https://docs.google.com/document/d/1EvXz-xlG7zcKYvzV3XL4bEfGkUxv2ByEln-h9N2nuxU/edit?usp=sharing "Anotações pessoais do curso")


### Migrations

CLI
dotnet ef migrations add PrimeiraMigracao -p Curso .\CursoEFCore -o PastaDeMigracoes -c AppContext

-p : Diretório do projeto


-o : Definir nome da pasta que as migrações são criadas (por padrão é criado em Migrations)


-c : Nome do contexto



Visual Studio

Add-Migration InitialCreate

### Dependências

- Microsoft.EntityFrameworkCore.Design

- Microsoft.EntityFrameworkCore.Sqlite

- Microsoft.EntityFrameworkCore.Tools

- Microsoft.Extensions.Logging.Abstractions

- Microsoft.Extensions.Logging.Console


