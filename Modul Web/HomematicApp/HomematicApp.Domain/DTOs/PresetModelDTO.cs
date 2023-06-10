using HomematicApp.Domain.Common;

namespace HomematicApp.Domain.DTOs
{
	public class PresetModelDTO
	{
        public string Device_Id { get; set; }
        public string Preset_Name { get; set; }
		public int Light { get; set; } 
		public float Temperature{ get; set; }
	}
}
