using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secret_Hitler_Backend.Models;
using Secret_Hitler_Backend.Databases;
using Secret_Hitler_Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController: ControllerBase
    {
        private SecretHitlerContext _context;
        public UserController(SecretHitlerContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        [Route("register")]
        public ActionResult<Boolean> RegisterUser(User user)
        {
            if(_context.Users == null)
            {
                return NotFound();
            }
            var result = _context.Users.Add(user);
            _context.SaveChanges();
            if(result.Entity != null)
            return Ok(true);
            return Ok(false);
        }
        [HttpPost]
        [Route("login")]
        public ActionResult<User> Login(UserCredentials userCred)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var result = _context.Users.FirstOrDefault(u => u.Name == userCred.Name  && u.Password == userCred.Password);
            return result;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            if(_context.Users == null)
            {
                return NotFound();
            }
            var results = _context.Users.ToList();
            return results;
        }

    }
}
