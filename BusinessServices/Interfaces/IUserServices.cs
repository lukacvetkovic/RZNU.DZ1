using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities.Entities;

namespace BusinessServices.Interfaces
{
    public interface IUserServices
    {
        int Authenticate(string email, string password);
        bool Register(string email, string password, string name);

        UserEntity GetUserByEmail(string email);
    }
}
