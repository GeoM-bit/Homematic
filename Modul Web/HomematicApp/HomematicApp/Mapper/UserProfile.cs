using AutoMapper;
using HomematicApp.Context.DbModels;
using HomematicApp.ViewModels;

namespace HomematicApp.Mapper
{
    public class UserProfile: Profile
    {
        public UserProfile() {
            CreateMap<UserModel, User>();
            CreateMap<LoginModel, LoginUser>();
            CreateMap<User, UserModel>().ForMember(dest=>dest.Passwrd, opt=>opt.Ignore());
        }
    }
}
