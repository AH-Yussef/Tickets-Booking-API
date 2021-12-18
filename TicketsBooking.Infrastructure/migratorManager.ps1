$migrationName =$args[0]
dotnet ef migrations add $migrationName --context TicketsBooking.Infrastructure.Persistence.AppDbContext -o Persistence/Migrations