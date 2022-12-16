using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entities;

namespace WebAPIAutores
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Autor> Autors { get; set; }
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
