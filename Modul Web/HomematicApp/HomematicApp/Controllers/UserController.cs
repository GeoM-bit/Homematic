using AutoMapper;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using HomematicApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        public async Task<IActionResult> ModifyParameters(ParameterModel parametersModel)
        {
            Parameter parameters = mapper.Map<Parameter>(parametersModel);
            await userRepository.modifyParameters(parameters,HttpContext.User.Identity.Name);

            return RedirectToAction("ViewParameters");
        }

        [HttpPost]
        public async Task<List<object>> GetChartData([FromBody] ChartDataModel chartDataModel)
        {
			var email = HttpContext.User.Identity.Name;
            var result = await userRepository.getChartData(email, (ActionType)Enum.Parse(typeof(ActionType),chartDataModel.action));
			
            return result;
		}
	}
}
