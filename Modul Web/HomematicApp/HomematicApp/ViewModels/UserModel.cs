using System.ComponentModel.DataAnnotations;

namespace HomematicApp.ViewModels
{
    public class UserModel
    {
        [Required(ErrorMessage = "IMEI code is required!")]
        public string Device_Id { get; set; }
        [Required(ErrorMessage = "First Name is required!")]
        public string First_Name { get; set; }
        [Required(ErrorMessage = "Last Name is required!")]
        public string Last_Name { get; set; }
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        public string? Passwrd { get; set; }
        [Required(ErrorMessage = "CNP is required!")]
        public string CNP { get; set; }
    }
}
