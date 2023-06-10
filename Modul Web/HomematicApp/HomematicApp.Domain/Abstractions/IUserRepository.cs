using HomematicApp.Context.DbModels;
using HomematicApp.Domain.DTOs;
using Action = HomematicApp.Context.DbModels.Action;

namespace HomematicApp.Domain.Abstractions
{
	public interface IUserRepository
	{
        Task<List<Action>> getActions(string email);
        Task<Parameter> getParameters();
        Task<bool> modifyParameters(Parameter parameter, string email);
        Task<List<PresetModelDTO>> getPresetList(string email);
		Task<List<object>> getChartData(string email, ActionType dataType);
        Task<bool> createPreset(PresetModelDTO preset, string email);
        Task<string> getCurrentPreset();
        Task<PresetModelDTO?> setPreset(string presetName, string email);
        Task<List<EspData>> getEspData();
	}
}
