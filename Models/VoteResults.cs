using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Models
{
    public class VoteResults
    {
        public bool Result { get; set; }
        public VoteScope Scope { get; set; }
        public List<Vote> Votes { get; set; }
    }
}
