using Microsoft.EntityFrameworkCore;
using Terraba.Models;

namespace Terraba.Data
{
    public class MediaReachContext : DbContext
    {
        public MediaReachContext(DbContextOptions<MediaReachContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}