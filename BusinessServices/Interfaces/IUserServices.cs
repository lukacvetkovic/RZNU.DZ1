using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces
{
    public interface IUserServices
    {
        int Authenticate(string email, string password);
        bool Register(string email, string password, string name);
    }
}
