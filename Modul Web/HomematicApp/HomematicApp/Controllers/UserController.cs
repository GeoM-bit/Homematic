using AutoMapper;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using HomematicApp.Domain.DTOs;
using HomematicApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

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
        public async Task<IActionResult> ViewActions(int? actionPage, int? espPage)
        {
            var email = HttpContext.User.Identity.Name;
            var actionsList = await userRepository.getActions(email);
            var actions = mapper.Map<List<ActionModel>>(actionsList);
            var espDataList = await userRepository.getEspData();
            var espData = mapper.Map<List<EspDataModel>>(espDataList);
           
            return View(new ActionListModel { Actions = actions.ToPagedList(actionPage ?? 1, 10), EspData = espData.ToPagedList(espPage ?? 1, 10) });
        }

        public async Task<IActionResult> ViewParameters()
        {
            var result = await userRepository.getParameters();
            ParameterModel model = mapper.Map<ParameterModel>(result);

            return View(model);
        }

        public async Task<IActionResult> CreatePreset(PresetListModel model)
        {
            if (model.Preset.Temperature == null || model.Preset.Preset_Name == null)
                return RedirectToAction("ViewPresets", new { PresetSuccess = false });
            var email = HttpContext.User.Identity.Name;
            var presetDTO = mapper.Map<PresetModelDTO>(model.Preset);
            var presetResult = await userRepository.createPreset(presetDTO, email);
            if (presetResult)
            {
                var parametersResult = await userRepository.modifyParameters(new Parameter
                                        {
                                            Light_Intensity = presetDTO.Light,
                                            Temperature = presetDTO.Temperature,
                                            Opened_Door = false,
                                            Current_Preset = presetDTO.Preset_Name
                                        }, email);
                if(parametersResult) {
                    return RedirectToAction("ViewPresets", new { PresetSuccess = true });
                }
            }
            return RedirectToAction("ViewPresets", new { PresetSuccess = false });
        }

        public async Task<IActionResult> ViewPresets()
        {
            var PresetSuccess = Request.Query["PresetSuccess"].ToString();
			var SetPresetSuccess = Request.Query["SetPresetSuccess"].ToString();
			if (!string.IsNullOrEmpty(PresetSuccess))
            {
                ViewBag.PresetSuccess = bool.Parse(PresetSuccess);
            }
			if (!string.IsNullOrEmpty(SetPresetSuccess))
			{
				ViewBag.SetPresetSuccess = bool.Parse(SetPresetSuccess);
			}
			var email = HttpContext.User.Identity.Name;
            var decodedList = await userRepository.getPresetList(email);
            var _presetNames = decodedList.Select(x=>x.Preset_Name).ToList();
            var currentPreset = await userRepository.getCurrentPreset();
            
            return View(new PresetListModel {
                Presets = decodedList,
                PresetNames=new(_presetNames),
                CurrentPreset = currentPreset
            });
        }

        public async Task<IActionResult> SetPreset(PresetListModel model)
        {
            var result = await userRepository.setPreset(model.SelectedPreset, HttpContext.User.Identity.Name);
            var preset = mapper.Map<PresetModelDTO>(result);
            if(result!=null)
            {
                var parametersResult = await userRepository.modifyParameters(new Parameter
                {
                    Light_Intensity = preset.Light,
                    Temperature = preset.Temperature,
                    Opened_Door = false,
                    Current_Preset = preset.Preset_Name
                }, HttpContext.User.Identity.Name);
                if (parametersResult)
                {
                    return RedirectToAction("ViewPresets", new { SetPresetSuccess = true });
                }
            }
            return RedirectToAction("ViewPresets", new { SetPresetSuccess = false });
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
