using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Models
{

        public enum Status
    {
        Pending, Normal, President, Chancellor, Dead,
        LastChancellor,
        LastPresident, NominatedChancellor
    }
    public enum Role
    {
        Spectator, Liberal, Fascist, Hitler
    }
    public enum VoteValue
    {
        Yes, No
    }
    public enum NotifType
    {
        Normal, Investigate, ChoosePresident, KillPlayer
    }
    public enum CardType
    {
        Fascist, Liberal
    }
    public enum CardsAction
    {
        Discard, Peek
    }
    public enum RoundState
    {
        Idle,PickPresident,Vote,
    }
}

