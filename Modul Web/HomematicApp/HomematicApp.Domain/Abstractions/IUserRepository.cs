using HomematicApp.Context.DbModels;

namespace HomematicApp.Domain.Abstractions
{
	public interface IUserRepository
	{
		Task<Parameters> GetParameters();
		Task<bool> Modify(Parameters parameters);
	}
}
