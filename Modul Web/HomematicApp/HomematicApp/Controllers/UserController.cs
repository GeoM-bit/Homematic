using AutoMapper;
using HomematicApp.Context.DbModels;
using HomematicApp.DataAccess.Repositories;
using HomematicApp.Domain.Abstractions;
using HomematicApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> CreateEvent(PresetModel model)
        {
            return View();
        }

        public async Task<IActionResult> ViewPresets()
        {
			var email = HttpContext.User.Identity.Name;
            var decodedList = await userRepository.getPresetList(email);
            var list = mapper.Map<List<PresetModel>>(decodedList);

			return View(new PresetListModel { Presets = list});
        }

        public async Task<IActionResult> ModifyParameters(ParametersModel parametersModel)
        {
            Parameters parameters = mapper.Map<Parameters>(parametersModel);
            var result = await userRepository.Modify(parameters);

            return RedirectToAction("ViewParameters");

        }
    }
}
