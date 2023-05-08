using HomematicApp.Context.Context;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using MySql.Data.MySqlClient;

namespace HomematicApp.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly HomematicContext _context;
        public AuthenticationRepository(HomematicContext context)
        {
            _context = context;
        }
        public Task<string> Login(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(User user)
        {
            
            user.Is_Admin = false;
            /*
            MySql.Data.MySqlClient.MySqlConnection conn;
            string conString = "server=127.0.0.1;database=homematic;uid=root;pwd=";
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = conString;
            conn.Open();

            MySqlCommand cmd = null;
            string cmdString = "";
            cmdString = "insert into users (device_id, password ,email ,first_name , last_name,is_admin, CNP) values('" + user.DeviceId + "','" + user.Password + "','" + user.Email + "','" + user.FirstName + "','" + user.LastName + "'," + user.IsAdmin + ",'" + user.CNP + "')";

            cmd = new MySqlCommand(cmdString, conn);
            int rowsAffected = cmd.ExecuteNonQuery();

            conn.Close();

            return rowsAffected == 1;*/
            _context.Users.Add(user);

            return await _context.SaveChangesAsync() == 1;

        }

        public Task<bool> ResetPassword(string deviceId, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
