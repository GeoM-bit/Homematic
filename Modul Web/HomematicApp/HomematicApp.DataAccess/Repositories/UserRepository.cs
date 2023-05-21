using HomematicApp.Context.Context;
using HomematicApp.Context.DbModels;
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

        public async Task<List<Action>> getActions(string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser == null) return null;
            return await _context.Actions.Where(a => a.Device_Id == dbUser.Device_Id).ToListAsync();
        }

		public void modifyParameters(Parameter parameter)
		{
			
		}

		public async Task<Parameter> getParameters()
		{
            var result = await _context.Parameters.ToListAsync();
            return result[0];
		}

	}
}
