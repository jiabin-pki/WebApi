using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.IServices;
using WebApi.Models;

namespace WebApi.Services
{
    public class UserService:IUserService
    {
        public readonly Context _context;
        public UserService(Context context)
        {
            _context = context;

        }

        public User GetUser(int id)
        {
            return _context.Users.Where(a => a.Id == id).FirstOrDefault();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void DelUser(int id)
        {
            var user = _context.Users.Where(a => a.Id == id).FirstOrDefault();
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        public User[] GetUsers()
        {
            return _context.Users.ToArray();
        }
        public void updateUser(int id, User user)
        {
            var olduser = _context.Users.Where(a => a.Id == id).FirstOrDefault();
            if (olduser != null)
            {
                _context.Remove(olduser);
                _context.Add(user);
                _context.SaveChanges();
            }
        }
    }
}
