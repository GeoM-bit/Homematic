using HomematicApp.Context.Context;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HomematicApp.DataAccess.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly HomematicContext _context;
		private readonly IHashService _hashService;
		private readonly IEmailSender _emailSender;
		private readonly ITemplateFillerService _templateFillerService;
		private readonly ITokenService _tokenService;
		private readonly IConfiguration _configuration;
		public AdminRepository(HomematicContext context, IHashService hashService, IEmailSender emailSender, ITemplateFillerService templateFillerService, ITokenService tokenService, IConfiguration configuration)
        {
            _context = context;
			_hashService = hashService;
			_emailSender = emailSender;
			_templateFillerService = templateFillerService;
			_tokenService = tokenService;
			_configuration = configuration;

		}
        public async Task<List<User>> GetUsers()
        {
			return await _context.Users.Where(u => u.Is_Admin == false).ToListAsync();
		}

        public async Task<bool> DeleteUser(string email)
        {

        var user=  await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            _context.Users.Remove(user);
            var result=await _context.SaveChangesAsync();
            return result==1?true:false;

        }

        public async Task<Parameters> GetParameters()
        { 
            var result= await _context.Parameters.ToListAsync();
			return result[0];
        }

        public async Task<bool> Modify(Parameters parameters)
        {
            parameters.Row_id = 1;
			_context.Parameters.Update(parameters);

			return await _context.SaveChangesAsync() == 1;
			
        }
	}
}
