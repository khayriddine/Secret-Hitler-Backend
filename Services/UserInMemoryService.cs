using System;
using System.Collections.Generic;
using Secret_Hitler_Backend.Models;
using System.Linq;
namespace Secret_Hitler_Backend.Services
{
    public class UserInMemoryService : IUserService{
        private IList<User> _users;

        public UserInMemoryService()
        {
            _users = new List<User>();
        }
        public bool CreateUser(User user){
            if (user == null)
                return false;
            _users.Add(user);
            return true;
        }
        public IEnumerable<User> GetAllUsers(){
            return _users;
        }
        public User GetUser(string name, string password)
        {
            if(_users != null && _users.Count >0)
            return _users.First(u => u.Name == name && u.Password == password);
            return null;
        }
    }
}