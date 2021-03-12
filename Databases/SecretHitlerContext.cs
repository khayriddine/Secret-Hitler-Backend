using Microsoft.EntityFrameworkCore;
using Secret_Hitler_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Databases
{
    public class SecretHitlerContext : DbContext
    {
        
        public DbSet<User> Users { get; set; }
        public SecretHitlerContext(DbContextOptions<SecretHitlerContext> options) : base(options)
        {
              
        }


    }
}
