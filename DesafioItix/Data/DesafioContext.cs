using Microsoft.EntityFrameworkCore;

namespace DesafioItix.Data
{
    public class DesafioContext : DbContext
    {
        public DesafioContext (DbContextOptions<DesafioContext> options)
            : base(options)
        {
        }

        public DbSet<DesafioItix.Models.Consulta> Consulta { get; set; }
    }
}
