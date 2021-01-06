using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DesafioItix.Models;

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
