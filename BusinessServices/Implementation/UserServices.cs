using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessServices.Interfaces;
using DataModel.UnitOfWork;

namespace BusinessServices.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly UnitOfWork _unitOfWork;


        public UserServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public int Authenticate(string email, string password)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Email == email && u.Password == password);
            if (user != null && user.Id > 0)
            {
                return user.Id;
            }
            return 0;
        }
    }
}
