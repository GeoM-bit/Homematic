using System.ComponentModel.DataAnnotations;

namespace HomematicApp.Context.DbModels
{
    public class User
    {
        [Key]
        public string Device_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Passwrd { get; set; }
        public bool Is_Admin { get; set; }
        public string CNP { get; set; }
    }
}
