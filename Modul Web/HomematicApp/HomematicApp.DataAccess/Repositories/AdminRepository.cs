using HomematicApp.Context.Context;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HomematicApp.DataAccess.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly HomematicContext _context;
		public AdminRepository(HomematicContext context)
        {
            _context = context;
		}
        public async Task<List<User>> GetUsers()
        {
            var result = await _context.Users.Where(u => u.Is_Admin == false).ToListAsync();
            return await _context.Users.Where(u => u.Is_Admin == false).ToListAsync();
		}

        public async Task<bool> DeleteUser(string email)
        {
            var user =  await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user != null)
            {
                var actions = await _context.Actions.Where(a => a.Device_Id == user.Device_Id).ToListAsync();
                var events = await _context.Presets.Where(p => p.Device_Id == user.Device_Id).ToListAsync();
				if (actions != null)
				{
					foreach (var action in actions)
						_context.Actions.Remove(action);
				}
				if (events != null)
				{
					foreach (var ev in events)
						_context.Presets.Remove(ev);
				}
			}
            if (user != null) {
                _context.Users.Remove(user);
            }          
            var result = await _context.SaveChangesAsync();
            return result !=0 ? true : false;
        }
	}
}
