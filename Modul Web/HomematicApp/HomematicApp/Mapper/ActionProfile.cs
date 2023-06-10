using AutoMapper;
using HomematicApp.ViewModels;
using Action = HomematicApp.Context.DbModels.Action;

namespace HomematicApp.Mapper
{
    public class ActionProfile : Profile
    {
        public ActionProfile()
        {
            CreateMap<Action, ActionModel>();
        }
    }
}
