using Action = HomematicApp.Context.DbModels.Action;

namespace HomematicApp.Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<List<Action>> getActions();
    }
}
