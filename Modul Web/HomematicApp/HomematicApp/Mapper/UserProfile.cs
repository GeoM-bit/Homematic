using AutoMapper;
using HomematicApp.Models;

namespace HomematicApp.Mapper
{
    public class UserProfile: Profile
    {
        public UserProfile() {
            CreateMap<UserModel, User>();
        }
    }
}
