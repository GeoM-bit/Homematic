using HomematicApp.Context.DbModels;
using Microsoft.EntityFrameworkCore;

namespace HomematicApp.Context.Context
{
    public class HomematicContext : DbContext
    {
        public HomematicContext(DbContextOptions<HomematicContext> options)
        : base(options)
        { }

        public DbSet<User> Users { get; set; }
		public DbSet<Parameters> Parameters { get; set; }

	}
}
