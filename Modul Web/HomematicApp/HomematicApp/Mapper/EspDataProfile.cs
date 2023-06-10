using AutoMapper;
using HomematicApp.Context.DbModels;
using HomematicApp.ViewModels;

namespace HomematicApp.Mapper
{
	public class EspDataProfile : Profile
	{
		public EspDataProfile() {
			CreateMap<EspData, EspDataModel>();
		}
	}
}
