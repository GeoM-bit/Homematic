using HomematicApp.Context.Context;
using HomematicApp.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Action = HomematicApp.Context.DbModels.Action;

namespace HomematicApp.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HomematicContext _context;
        public UserRepository(HomematicContext context)
        {
            _context = context;
        }

        public async Task<List<Action>> getActions()
        {
            return await _context.Actions.ToListAsync();
        }
    }
}
