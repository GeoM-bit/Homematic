using HomematicApp.Context.DbModels;
using HomematicApp.Domain.DTOs;
using Action = HomematicApp.Context.DbModels.Action;

namespace HomematicApp.Domain.Abstractions
{
	public interface IUserRepository
	{
        Task<List<Action>> getActions(string email);
        Task<Parameter> getParameters();
        Task<bool> modifyParameters(Parameter parameter);
        Task<List<PresetModelDTO>> getPresetList(string email);
        PresetModelDTO decodePreset(Preset preset);
	}
}
