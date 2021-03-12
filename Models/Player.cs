using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Models
{
    public class Player
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }
        public Status Status { get; set; }
        public Role Role { get; set; }
        public string ProfileImg { get; set; }
    }
}
