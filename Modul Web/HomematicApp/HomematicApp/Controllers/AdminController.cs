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

       // [Route("Admin/DeleteUser/{email}")]
        public async Task<IActionResult> DeleteUser([FromBody]object? email)
		{
            //string email = (string)HttpContext.GetRouteData().Values["email"];
            var res = HttpContext.Request.Body.ToString();
            //var result=await adminRepository.DeleteUser(email);
			
			return RedirectToAction("ViewUsers");
		}

	}
}
