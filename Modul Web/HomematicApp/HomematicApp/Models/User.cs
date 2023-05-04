using System.ComponentModel.DataAnnotations;

namespace HomematicApp.Models
{
    public class User
    {
        [Key]
        public string DeviceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? IsAdmin { get; set; }
        public string CNP { get; set; }
    }
}
