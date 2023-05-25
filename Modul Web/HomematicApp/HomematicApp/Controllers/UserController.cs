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
			this.userRepository = _userRepository;
			mapper = _mapper;
		}
		public async Task<IActionResult> ViewParameters()
		{
			var result = await userRepository.GetParameters();
			var model = mapper.Map<ParametersModel>(result);
			return View(model);
		}

		public async Task<IActionResult> ModifyParameters(ParametersModel parametersModel)
		{
			Parameters parameters = mapper.Map<Parameters>(parametersModel);
			var result = await userRepository.Modify(parameters);

			return RedirectToAction("ViewParameters");

		}
	}
}
