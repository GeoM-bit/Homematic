using HomematicApp.Context.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomematicApp.Domain.Abstractions
{
    public interface IAdminRepository
    {
        Task<List<User>> GetUsers();
    }
}
