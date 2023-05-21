using HomematicApp.Domain.Common;

namespace HomematicApp.Domain.DTOs
{
	public class PresetModelDTO
	{
		public int Preset_Id { get; set; }
		public string Preset_Name { get; set; }
		public string Device_Id { get; set; }
		public DateTime Start_Date { get; set; }
		public DateTime End_Date { get; set; }
		public List<Options> Light_Options { get; set; } 
		public List<Options> Temperature_Options { get; set; }
	}
}
