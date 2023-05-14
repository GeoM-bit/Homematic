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
            return await _context.Users.ToListAsync();
        }
    }
}
