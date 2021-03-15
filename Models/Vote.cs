using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Models
{
    public class Vote
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public VoteValue VoteValue { get; set; }
    }
}
