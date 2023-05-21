using AutoMapper;
using HomematicApp.Domain.Abstractions;
using HomematicApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var email = HttpContext.User.Identity.Name;
            var list = await userRepository.getActions(email);
            var actions = mapper.Map<List<ActionModel>>(list);

            return View(new ActionListModel { Actions = actions});
        }

        public async Task<IActionResult> ViewParameters()
        {
            var result = await userRepository.getParameters();
            ParameterModel model = mapper.Map<ParameterModel>(result);

            return View(model);
        }
    }
}
