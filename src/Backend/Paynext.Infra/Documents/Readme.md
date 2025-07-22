cd C:\Dev\Paynext\src\Backend\Paynext.Infra

dotnet ef migrations add InitialCreate -o Migrations

dotnet ef database update