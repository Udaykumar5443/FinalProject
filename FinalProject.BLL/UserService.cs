using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.DAL;
using FinalProject.Models;

namespace FinalProject.BLL
{
    public class UserService
    {
        private readonly UserDAL _userDAL;

        public UserService()
        {
            _userDAL = new UserDAL();
        }

        public bool RegisterUser(User user)
        {
            return _userDAL.RegisterUser(user);
        }

        public User AuthenticateUser(string username, string password)
        {
            return _userDAL.ValidateUser(username, password);
        }
    }
}
