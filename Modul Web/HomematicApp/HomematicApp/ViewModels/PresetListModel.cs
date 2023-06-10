using HomematicApp.Domain.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomematicApp.ViewModels
{
	public class PresetListModel
	{
		public List<PresetModelDTO> Presets { get; set; }
		public PresetModel Preset { get; set; }
		public string CurrentPreset { get; set; }
		public SelectList PresetNames { get; set; } 
		public string? SelectedPreset { get; set; }
	}
}
