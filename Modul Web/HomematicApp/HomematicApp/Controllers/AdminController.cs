using AutoMapper;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using HomematicApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HomematicApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository adminRepository;
        private readonly IMapper mapper;
        public AdminController(IAdminRepository _adminRepository, IMapper _mapper)
        {
            adminRepository = _adminRepository;
            mapper = _mapper;
        }
        public async Task<IActionResult> ViewUsers()
        {
            var result= await adminRepository.GetUsers();
            var list = mapper.Map<List<UserModel>>(result);
            var viewUsersModel=new ViewUsersModel { ViewUsers = list };
            return View(viewUsersModel);
        }

		//[Route("Admin/DeleteUser/{email}")]
		
		public async Task<IActionResult> DeleteUser()
		{
          /// var email1 = (string)HttpContext.GetRouteData().Values["email"];
            
          //var res = HttpContext.Request.Body.ToString();
           // var result=await adminRepository.DeleteUser(email1);
			
			return RedirectToAction("ViewUsers");
		}

		public async Task<IActionResult> ViewParameters()
		{
			var result = await adminRepository.GetParameters();
			var list = mapper.Map<List<ParametersModel>>(result);
			var viewParametersModel = new ViewParametersModel { ViewParameters = list };
			return View(viewParametersModel);
		}

	}
}
