using Org.BouncyCastle.Asn1.Mozilla;

namespace HomematicApp.Models
{
    public class UserModel
    {
        public string DeviceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CNP { get; set; }

        public bool? FailedRegister { get; set; }

    }
}
