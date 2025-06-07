using EvaluadorInteligente.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EvaluadorInteligente.Data;

public class ApplicationDbContextFactory
           : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // 👉  cadena directa SOLO para las migraciones
        var conn = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=USMP2020";

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseNpgsql(conn);

        return new ApplicationDbContext(builder.Options);
    }
}
