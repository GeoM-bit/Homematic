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

        public async Task<bool> DeleteUser(string email)
        {

        var user=  await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            _context.Users.Remove(user);
            var result=await _context.SaveChangesAsync();
            return result==1?true:false;

        }

        public async Task<List<Parameters>> GetParameters()
        {
            return await _context.Parameters.ToListAsync();
        }

    }
}
