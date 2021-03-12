using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Models
{
    public class Room
    {
        public string Name { get; set; }
        public int RoomId { get; set; }
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public int Size { get; set; }
        public List<User> Users { get; set; }
    }
}
