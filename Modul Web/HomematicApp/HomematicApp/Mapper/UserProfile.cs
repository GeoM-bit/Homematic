using AutoMapper;
using HomematicApp.Context.DbModels;
using HomematicApp.ViewModels;

namespace HomematicApp.Mapper
{
    public class UserProfile: Profile
    {
        public UserProfile() {
            CreateMap<UserModel, User>();
        }
    }
}
