using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Models
{
    public class GameResult
    {
        public int GameResultId { get; set; }
        public List<Player> Fascists { get; set; }
        public string Reason { get; set; }
        public CardType WinningTeam { get; set; }
    }
}
