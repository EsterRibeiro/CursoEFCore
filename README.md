# CursoEFCore
Curso de Entity Framework Core da plataforma Desenvolvedor.IO

Material:




[Connection Strings](https://www.connectionstrings.com/ "Connection Strings Homepage")

Migrations

CLI
dotnet ef migrations add PrimeiraMigracao -p Curso .\CursoEFCore -o PastaDeMigracoes -c AppContext

-p : Diretório do projeto
-o : Definir nome da pasta que as migrações são criadas (por padrão é criado em Migrations)
-c : Nome do contexto

Visual Studio

Add-Migration InitialCreate

