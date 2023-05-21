using HomematicApp.Context.DbModels;
using Microsoft.EntityFrameworkCore;
using Action = HomematicApp.Context.DbModels.Action;

namespace HomematicApp.Context.Context
{
    public class HomematicContext : DbContext
    {
        public HomematicContext(DbContextOptions<HomematicContext> options)
        : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
    }
}
