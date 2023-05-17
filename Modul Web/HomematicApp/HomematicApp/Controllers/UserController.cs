using AutoMapper;
using HomematicApp.Domain.Abstractions;
using HomematicApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Action = HomematicApp.Context.DbModels.Action;

namespace HomematicApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public UserController(IUserRepository _userRepository, IMapper _mapper)
        {
            userRepository = _userRepository;
            mapper = _mapper;
        }
        public async Task<IActionResult> ViewActions()
        {
            var list = await userRepository.getActions();
            var actions = mapper.Map<List<ActionModel>>(list);

            return View(new ActionListModel { Actions = actions});
        }
    }
}
