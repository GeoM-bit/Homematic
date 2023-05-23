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
			var DeleteSuccessful = Request.Query["DeleteSuccessful"].ToString();
			if (!string.IsNullOrEmpty(DeleteSuccessful))
			{
				ViewBag.DeleteSuccessful = bool.Parse(DeleteSuccessful);
			}
			var result= await adminRepository.GetUsers();
            var list = mapper.Map<List<UserModel>>(result);
            var viewUsersModel=new ViewUsersModel { ViewUsers = list };
            return View(viewUsersModel);
        }

		[HttpPost]
		public async Task<IActionResult> DeleteUser([FromBody] DeleteModel deleteModel) 
		{
           var result=await adminRepository.DeleteUser(deleteModel.Email);
			
			if(result)
			    return RedirectToAction("ViewUsers", new { DeleteSuccessful = true });
            else
				return RedirectToAction("ViewUsers", new { DeleteSuccessful = false });

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
