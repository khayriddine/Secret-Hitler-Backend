using Secret_Hitler_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Databases
{
    public static class DbInitializer
    {
        public static void Initialize(SecretHitlerContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }
            var users = new User[]
{
            new User{Name="Carson",Password="Alexander",ProfileImg=""},
            new User{Name="Meredith",Password="Alonso",ProfileImg=""},
            new User{Name="Arturo",Password="Anand",ProfileImg=""},
            new User{Name="Gytis",Password="Barzdukas",ProfileImg=""},
            new User{Name="Yan",Password="Li",ProfileImg=""},
            new User{Name="Peggy",Password="Justice",ProfileImg=""},
            new User{Name="Laura",Password="Norman",ProfileImg=""},
            new User{Name="Nino",Password="Olivetto",ProfileImg=""}
};
            foreach (User usr in users)
            {
                context.Users.Add(usr);
            }
            context.SaveChanges();
        }
    }
}
