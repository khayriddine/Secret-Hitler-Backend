using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Models
{
    public class GameRound
    {
        private Dictionary<int, VoteValue> _votes = new Dictionary<int, VoteValue>();
        public int PresidentId { get; set; }
        public int LastPresidentId { get; set; }
        public int ChancellorId { get; set; }
        public int LastChancellorId { get; set; }
        public int NominatedChancellorId { get; set; }
        public RoundState RoundState { get; set; }
        public List<VoteValue> Votes { get { return _votes.Values.ToList(); } }
        public GameRound()
        {
            PresidentId = -1;
            LastPresidentId = -1;
            ChancellorId = -1;
            LastChancellorId = -1;
            NominatedChancellorId = -1;
            RoundState = RoundState.Idle;
            _votes = new Dictionary<int, VoteValue>();
        }

        public void MoveToNextRound()
        {
            PresidentId = -1;
            LastPresidentId = PresidentId;
            ChancellorId = -1;
            LastChancellorId = ChancellorId;
            NominatedChancellorId = -1;
            RoundState = RoundState.Idle;
            Votes.Clear();
        }

        public void NominateChancellor(int chancId)
        {
            try
            {
                LastChancellorId = ChancellorId;
                NominatedChancellorId = chancId;
                ChancellorId = -1;
                RoundState = RoundState.Vote;
                _votes.Clear();
            }
            catch (Exception ex )
            {
                Console.WriteLine("NominateChancellor.ex: "+ ex.Message);
            }
        }
    }
}
