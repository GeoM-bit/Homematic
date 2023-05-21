using AutoMapper;
using HomematicApp.Domain.DTOs;
using HomematicApp.ViewModels;

namespace HomematicApp.Mapper
{
	public class PresetProfile : Profile
	{
		public PresetProfile()
		{
			CreateMap<PresetModelDTO, PresetModel>();
		}
	}
}
