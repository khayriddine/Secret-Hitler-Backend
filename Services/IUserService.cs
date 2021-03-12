using System;
using System.Collections.Generic;
using Secret_Hitler_Backend.Models;
namespace Secret_Hitler_Backend.Services
{

    public interface IUserService
    {
        bool CreateUser(User user);
        IEnumerable<User> GetAllUsers();
        User GetUser(string name, string password);
    }
}