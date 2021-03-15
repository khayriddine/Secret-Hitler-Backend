using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Models
{
    public class GameRound
    {
        private Dictionary<int, Vote> _votes = new Dictionary<int, Vote>();
        public int PresidentId { get; set; }
        public int LastPresidentId { get; set; }
        public int ChancellorId { get; set; }
        public int LastChancellorId { get; set; }
        public int NominatedChancellorId { get; set; }
        public RoundState RoundState { get; set; }
        public List<Card> CardsInHand { get; set; }
        public List<Vote> Votes { get { return _votes.Values.ToList(); } }
        public int NbreOfFailedVotes { get; set; }
        public VoteResults VoteResults { get; set; }
        public GameRound()
        {
            PresidentId = -1;
            LastPresidentId = -1;
            ChancellorId = -1;
            LastChancellorId = -1;
            NominatedChancellorId = -1;
            NbreOfFailedVotes = 0;
            RoundState = RoundState.Start;
            _votes = new Dictionary<int, Vote>();
            CardsInHand = new List<Card>();
        }

        public void MoveToNextRound()
        {
            LastPresidentId = PresidentId;
            PresidentId = -1;

            LastChancellorId = ChancellorId;
            ChancellorId = -1;
            
            NominatedChancellorId = -1;
            RoundState = RoundState.PickChancellor;
            _votes.Clear();
            CardsInHand.Clear();
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

        public bool IsPlayerAlreadyVoted(int userId)
        {
            return _votes.ContainsKey(userId);
        }

        public void AddVote(Vote vote)
        {
            try
            {
                _votes[vote.UserId] = vote;
            }
            catch (Exception ex)
            {
                Console.WriteLine("AddVote.ex: "+ ex.Message);
            }
        }

        internal void ElectChancellor()
        {
            try
            {
                ChancellorId = NominatedChancellorId;
                NominatedChancellorId = -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ElectChancellor.ex: "+ex.Message);
            }
        }

        public  void PreparePresidentToDiscard(List<Card> cards)
        {
            try
            {
                RoundState = RoundState.PresidentDiscard;
                CardsInHand.AddRange(cards);
            }
            catch (Exception ex)
            {
                Console.WriteLine("PreparePresidentToDiscard.ex: "+ex.Message);
            }
        }

        public void ForceCard(Card card)
        {
            try
            {
                RoundState = RoundState.ForcedCard;
                CardsInHand.Add(card);
                NbreOfFailedVotes = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ForceCard.ex: "+ex.Message);
            }
        }

        public void DiscardCard(int discarded)
        {
            try
            {
                var cardInd = CardsInHand.FindIndex(c => c.CardId == discarded);
                if(cardInd >= 0)
                {
                    CardsInHand.RemoveAt(cardInd);

                }
                if(CardsInHand.Count == 2)
                {
                    RoundState = RoundState.ChancellorDiscard;
                }
                else if(CardsInHand.Count == 1)
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DiscardCard.ex :"+ex.Message);
            }
        }
    }
}
