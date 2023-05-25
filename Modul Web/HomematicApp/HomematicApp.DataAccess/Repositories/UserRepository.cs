using HomematicApp.Context.Context;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace HomematicApp.DataAccess.Repositories
{
	public class UserRepository:IUserRepository
	{
		private readonly HomematicContext _context;
		public UserRepository(HomematicContext context)
		{
			_context = context;
		}
		public async Task<Parameters> GetParameters()
		{
			var result = await _context.Parameters.ToListAsync();
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
