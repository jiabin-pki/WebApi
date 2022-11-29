using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.IServices
{
    public interface IUserService
    {
        public User GetUser(int id);

        public void AddUser(User user);

        public void DelUser(int id);

        public User[] GetUsers();
        public void updateUser(int id, User user);


    }
}
