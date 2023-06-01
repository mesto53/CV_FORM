using ass3.Pages.Model;
using Microsoft.EntityFrameworkCore;


namespace ass3.Pages.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        public DbSet<CVMODELDB> CVMODEL { get; set; }
    }
}
