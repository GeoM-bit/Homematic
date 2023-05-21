using HomematicApp.Context.DbModels;
using Action = HomematicApp.Context.DbModels.Action;

namespace HomematicApp.Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<List<Action>> getActions(string email);
        Task<Parameter> getParameters();
        void modifyParameters(Parameter parameter);
    }
}
