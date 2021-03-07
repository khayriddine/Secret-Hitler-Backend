using System;

namespace Models{
    public class User{
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ProfileImg { get; set; }
    }

    public class UserCredentials
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}