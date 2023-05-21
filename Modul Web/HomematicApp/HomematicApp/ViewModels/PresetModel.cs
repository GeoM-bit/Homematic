using HomematicApp.Domain.Common;

namespace HomematicApp.ViewModels
{
	public class PresetModel
	{
		public string? Preset_Name { get; set; }
		public DateTime Start_Date { get; set; }
		public DateTime End_Date { get; set; }
		public List<Options>? Light_Options { get; set; }
		public List<Options>? Temperature_Options { get; set; }
		public Options? Light {  get; set; }
		public Options? Temperature { get; set; }

	}
}
