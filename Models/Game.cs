using Secret_Hitler_Backend.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Models
{
    public class Game
    {
        private Dictionary<int, Player> playersDict = new Dictionary<int, Player>();
        private int presidentIndex = 0;
        private int fascistCardPlacedNbre = 0;
        public int RoomId { get; set; }
        public List<Player> Players { get { return playersDict.Values.ToList(); } }
        public List<Player> AlivePlayers { get { return playersDict.Values.Where(p =>p.Status != Status.Dead).ToList(); } }
        public GameRound CurrentRound { get; set; }
        
        public List<Card> RemainingCards { get; set; }
        public List<Card> DiscardedCards { get; set; }
        public List<Card> CardsOnBoard { get; set; }
        public GameResult GameResult { get; set; }
        [JsonIgnore]
        public bool AllPalyerVoted { 
            get {
                
                if (AlivePlayers != null && CurrentRound.Votes != null)
                    return AlivePlayers.Count == CurrentRound.Votes.Count;
                return false;
            } }
        [JsonIgnore]
        public int FascistCardsOnBoard { get { return CardsOnBoard.Where(c => c.CardType == CardType.Fascist).Count(); } }



        public Game(Room room)
        {
            if(room != null)
            {
                RoomId = room.RoomId;
                PreparePlayers(room.Users);
                PrepareCards();


                PrepareGame();

            }
        }

        private void PreparePlayers(List<User> users)
        {
            try
            {

                switch (users.Count)
                {
                    case 5:
                        foreach (var u in users)
                        {
                            playersDict.Add(u.UserId, new Player()
                            {
                                Name = u.Name,
                                ProfileImg = u.ProfileImg,
                                UserId = u.UserId,
                                Status = Status.Pending,
                                RoomId = RoomId
                            });
                        }
                        Players[0].Role = Role.Hitler;
                        Players[1].Role = Role.Fascist;
                        Players[2].Role = Role.Liberal;
                        Players[3].Role = Role.Liberal;
                        Players[4].Role = Role.Liberal;
                        break;
                    case 6:
                        foreach (var u in users)
                        {
                            playersDict.Add(u.UserId, new Player()
                            {
                                Name = u.Name,
                                ProfileImg = u.ProfileImg,
                                UserId = u.UserId,
                                Status = Status.Pending,
                                RoomId = RoomId
                            });
                        }
                        Players[0].Role = Role.Hitler;
                        Players[1].Role = Role.Fascist;
                        Players[2].Role = Role.Liberal;
                        Players[3].Role = Role.Liberal;
                        Players[4].Role = Role.Liberal;
                        Players[5].Role = Role.Liberal;
                        break;
                    case 7:
                        foreach (var u in users)
                        {
                            playersDict.Add(u.UserId, new Player()
                            {
                                Name = u.Name,
                                ProfileImg = u.ProfileImg,
                                UserId = u.UserId,
                                Status = Status.Pending,
                                RoomId = RoomId
                            });
                        }
                        Players[0].Role = Role.Hitler;
                        Players[1].Role = Role.Fascist;
                        Players[2].Role = Role.Fascist;
                        Players[3].Role = Role.Liberal;
                        Players[4].Role = Role.Liberal;
                        Players[5].Role = Role.Liberal;
                        Players[6].Role = Role.Liberal;
                        break;
                    case 8:
                        foreach (var u in users)
                        {
                            playersDict.Add(u.UserId, new Player()
                            {
                                Name = u.Name,
                                ProfileImg = u.ProfileImg,
                                UserId = u.UserId,
                                Status = Status.Pending,
                                RoomId = RoomId
                            });
                        }
                        Players[0].Role = Role.Hitler;
                        Players[1].Role = Role.Fascist;
                        Players[2].Role = Role.Fascist;
                        Players[3].Role = Role.Liberal;
                        Players[4].Role = Role.Liberal;
                        Players[5].Role = Role.Liberal;
                        Players[6].Role = Role.Liberal;
                        Players[7].Role = Role.Liberal;
                        break;
                    case 9:
                        foreach (var u in users)
                        {
                            playersDict.Add(u.UserId, new Player()
                            {
                                Name = u.Name,
                                ProfileImg = u.ProfileImg,
                                UserId = u.UserId,
                                Status = Status.Pending,
                                RoomId = RoomId
                            });
                        }
                        Players[0].Role = Role.Hitler;
                        Players[1].Role = Role.Fascist;
                        Players[2].Role = Role.Fascist;
                        Players[3].Role = Role.Fascist;
                        Players[4].Role = Role.Liberal;
                        Players[5].Role = Role.Liberal;
                        Players[6].Role = Role.Liberal;
                        Players[7].Role = Role.Liberal;
                        Players[8].Role = Role.Liberal;
                        break;
                    case 10:
                        foreach (var u in users)
                        {
                            playersDict.Add(u.UserId, new Player()
                            {
                                Name = u.Name,
                                ProfileImg = u.ProfileImg,
                                UserId = u.UserId,
                                Status = Status.Pending,
                                RoomId = RoomId
                            });
                        }
                        Players[0].Role = Role.Hitler;
                        Players[1].Role = Role.Fascist;
                        Players[2].Role = Role.Fascist;
                        Players[3].Role = Role.Fascist;
                        Players[4].Role = Role.Fascist;
                        Players[5].Role = Role.Liberal;
                        Players[6].Role = Role.Liberal;
                        Players[7].Role = Role.Liberal;
                        Players[8].Role = Role.Liberal;
                        Players[9].Role = Role.Liberal;
                        break;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("PreparePlayers.ex: "+ex.Message);
            }
        }

        public void NominateChancellor(int chancId)
        {
            try
            {
                if(CurrentRound != null)
                {
                    CurrentRound.NominateChancellor(chancId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CanNominateChancellor.ex: "+ ex.Message);
            }
        }

        private void PrepareCards()
        {
            RemainingCards = new List<Card>();
            for (int i = 0; i < 6; i++)
                RemainingCards.Add(new Card() { CardId = i, CardType = CardType.Fascist });
            for (int i = 6; i < 17; i++)
                RemainingCards.Add(new Card() { CardId = i, CardType = CardType.Liberal });

            CardsOnBoard = new List<Card>();
            DiscardedCards = new List<Card>();
        }

        private void PrepareGame()
        {
            Players.Shuffle();
            RemainingCards.Shuffle();

            CurrentRound = new GameRound();
            
            foreach (var p in Players)
                p.Status = Status.Normal;
            CurrentRound.RoundState = RoundState.PickChancellor;
            CurrentRound.PresidentId = Players[0].UserId;
            Players[0].Status = Status.President;
        }
        public bool CanNominateChancellor(int prezId,int chancId)
        {
            try
            {
                if(CurrentRound != null && playersDict[prezId].Status != Status.Dead && playersDict[chancId].Status != Status.Dead)
                {
                    var nbreAlives = Players.Where(p => p.Status != Status.Dead).Count();
                    if (nbreAlives >= 6 && chancId == CurrentRound.LastPresidentId)
                    {
                        return false;
                    }
                    else if (chancId == CurrentRound.LastChancellorId)
                        return false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CanNominateChancellor.ex: "+ ex.Message);
            }
            return false;
        }

        public bool CanPlayerVote(Vote vote)
        {
            try
            {
                if (playersDict.ContainsKey(vote.UserId))
                {
                    var player = playersDict[vote.UserId];
                    if(player.Status == Status.Dead)
                    {
                        return false;
                    }
                    else if (!CurrentRound.IsPlayerAlreadyVoted(vote.UserId))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CanPlayerVote.ex :"+ex.Message) ;
            }
            return false;
        }

        public void ProceedAfterVote()
        {
            try
            {
                if (CurrentRound.VoteResults.Result)
                {
                    //Update round:
                    CurrentRound.ElectChancellor();
                    if(FascistCardsOnBoard >= 3 && playersDict[CurrentRound.ChancellorId].Role == Role.Hitler)
                    {
                        CurrentRound.RoundState = RoundState.End;
                        GameResult = new GameResult()
                        {
                            Fascists = Players.Where(p => p.Role != Role.Liberal).ToList(),
                            WinningTeam = CardType.Fascist,
                            Reason = "Hitler elected after three fascit Cards Placed on Board"
                        };
                    }
                    else
                    {
                        if(RemainingCards.Count < 3)
                        {
                            DiscardedCards.Shuffle();
                            RemainingCards.AddRange(DiscardedCards);
                            DiscardedCards.Clear();
                        }
                        var cards = new List<Card>() { RemainingCards[0], RemainingCards[1], RemainingCards[2] };
                        RemainingCards.RemoveRange(0, 3);
                        CurrentRound.PreparePresidentToDiscard(cards);


                    }
                }
                else
                {
                    CurrentRound.NbreOfFailedVotes++;
                    if(CurrentRound.NbreOfFailedVotes == 3)
                    {
                        if(RemainingCards.Count < 1)
                        {
                            DiscardedCards.Shuffle();
                            RemainingCards.AddRange(DiscardedCards);
                            DiscardedCards.Clear();
                        }
                        Card card = RemainingCards[0];

                        RemainingCards.RemoveAt(0);
                        CardsOnBoard.Add(card);

                        CurrentRound.NbreOfFailedVotes = 0;
                        MakeActionBeforeEnd();

                    }
                    else
                    {
                        MoveToNextTurn();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ProceedAfterVote.ex : "+ex.Message);
            }
        }
        public void MakeActionBeforeEnd()
        {
            try
            {
                if (FascistCardsOnBoard == 6)
                {
                    CurrentRound.RoundState = RoundState.End;
                    GameResult = new GameResult()
                    {
                        Fascists = Players.Where(p => p.Role != Role.Liberal).ToList(),
                        WinningTeam = CardType.Fascist,
                        Reason = "Six fascit Cards Placed on Board"
                    };
                }
                else if (CardsOnBoard.Where(c => c.CardType == CardType.Liberal).Count() == 5)
                {
                    CurrentRound.RoundState = RoundState.End;
                    GameResult = new GameResult()
                    {
                        Fascists = Players.Where(p => p.Role != Role.Liberal).ToList(),
                        WinningTeam = CardType.Fascist,
                        Reason = "Six fascit Cards Placed on Board"
                    };
                }
                else if (Players.Count == 9 || Players.Count == 10)
                {
                    if (FascistCardsOnBoard == 1 && fascistCardPlacedNbre == 0)
                    {
                        fascistCardPlacedNbre++;
                        CurrentRound.RoundState = RoundState.InvestigateMembership;
                    }
                    else if(FascistCardsOnBoard == 2 && fascistCardPlacedNbre == 1)
                    {
                        fascistCardPlacedNbre++;
                        CurrentRound.RoundState = RoundState.InvestigateMembership;
                    }
                    else if (FascistCardsOnBoard == 3 && fascistCardPlacedNbre == 2)
                    {
                        fascistCardPlacedNbre++;
                        CurrentRound.RoundState = RoundState.PickNextPresident;
                    }
                    else if (FascistCardsOnBoard == 4 && fascistCardPlacedNbre == 3)
                    {
                        CurrentRound.RoundState = RoundState.KillMember;
                    }
                    else if (FascistCardsOnBoard == 5 && fascistCardPlacedNbre == 4)
                    {
                        CurrentRound.RoundState = RoundState.KillMember;
                    }
                    else
                    {
                        MoveToNextTurn();
                    }
                }
                else if (Players.Count == 7 || Players.Count == 8)
                {
                    if (FascistCardsOnBoard == 2 && fascistCardPlacedNbre == 0)
                    {
                        fascistCardPlacedNbre++;
                        CurrentRound.RoundState = RoundState.InvestigateMembership;
                    }
                    else if (FascistCardsOnBoard == 3 && fascistCardPlacedNbre == 1)
                    {
                        fascistCardPlacedNbre++;
                        CurrentRound.RoundState = RoundState.PickNextPresident;
                    }
                    else if (FascistCardsOnBoard == 4 && fascistCardPlacedNbre == 2)
                    {
                        CurrentRound.RoundState = RoundState.KillMember;
                    }
                    else if (FascistCardsOnBoard == 5 && fascistCardPlacedNbre == 3)
                    {
                        CurrentRound.RoundState = RoundState.KillMember;
                    }
                    else
                    {
                        MoveToNextTurn();
                    }
                }
                else 
                {
                    if (FascistCardsOnBoard == 3 && fascistCardPlacedNbre == 0)
                    {
                        fascistCardPlacedNbre++;
                        CurrentRound.RoundState = RoundState.PeekNextCards;
                        CurrentRound.CardsInHand.Clear();
                        if (RemainingCards.Count < 3)
                        {
                            DiscardedCards.Shuffle();
                            RemainingCards.AddRange(DiscardedCards);
                            DiscardedCards.Clear();
                        }
                        CurrentRound.CardsInHand.Add(RemainingCards[0]);
                        CurrentRound.CardsInHand.Add(RemainingCards[1]);
                        CurrentRound.CardsInHand.Add(RemainingCards[2]);
                    }
                    else if (FascistCardsOnBoard == 4 && fascistCardPlacedNbre == 1)
                    {
                        CurrentRound.RoundState = RoundState.KillMember;
                    }
                    else if (FascistCardsOnBoard == 5 && fascistCardPlacedNbre == 2)
                    {
                        CurrentRound.RoundState = RoundState.KillMember;
                    }
                    else
                    {
                        MoveToNextTurn();
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("ProceedAfterDiscard.ex: "+ ex.Message);
            }
        }

        public void PickNextPresident(int playerId)
        {
            try
            {
                CurrentRound.MoveToNextRound();
                CurrentRound.PresidentId = playerId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("PickNextPresident.ex: " + ex.Message);
            }
        }

        public void KillPlayer(int playerId)
        {
            try
            {
                if (playersDict.ContainsKey(playerId))
                {
                    var p = playersDict[playerId];
                    p.Status = Status.Dead;
                    if(p.Role == Role.Hitler)
                    {
                        CurrentRound.RoundState = RoundState.End;
                        GameResult = new GameResult()
                        {
                            Fascists = Players.Where(p => p.Role != Role.Liberal).ToList(),
                            WinningTeam = CardType.Fascist,
                            Reason = "Hitler got killed !!"
                        };
                    }
                    else
                    {
                        MoveToNextTurn();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("KillPlayer.ex: " + ex.Message);
            }
        }

        public Player GetPlayerById(int playerId)
        {
            Player player = null;
            try
            {
                if (playersDict.ContainsKey(playerId))
                    player = playersDict[playerId];
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPlayerById.ex: "+ex.Message);
            }
            return player;
        }

        public void DiscardCard(int discarded)
        {
            try
            {
                if(CurrentRound != null)
                {
                    var cardInd = CurrentRound.CardsInHand.FindIndex(c => c.CardId == discarded);
                    if (cardInd >= 0)
                    {
                        var card = CurrentRound.CardsInHand[cardInd];
                        DiscardedCards.Add(card);
                        CurrentRound.CardsInHand.RemoveAt(cardInd);

                    }
                    if (CurrentRound.CardsInHand.Count == 2)
                    {
                        CurrentRound.RoundState = RoundState.ChancellorDiscard;
                    }
                    else if (CurrentRound.CardsInHand.Count == 1)
                    {
                        var card = CurrentRound.CardsInHand[0];
                        CardsOnBoard.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DiscardCard.ex: "+ex.Message);
            }
        }

        public void CalculateVoteResults(VoteScope scope)
        {
            var voteResults = new VoteResults();
            try
            {
                
                voteResults.Scope = scope;
                switch (scope)
                {
                    case VoteScope.Chancellor:
                        {
                            voteResults.Result = ((CurrentRound.Votes.Where(v => v.VoteValue == VoteValue.Yes).Count()) > AlivePlayers.Where(p => p.Role == Role.Liberal).Count() / 2) ;
                            voteResults.Votes = CurrentRound.Votes;
                        }
                        break;
                    case VoteScope.Veto:
                        {
                            voteResults.Result = CurrentRound.Votes[0].VoteValue == VoteValue.Yes;
                        }break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CalculateVoteResults.ex : "+ex.Message);
            }
            CurrentRound.VoteResults = voteResults;
        }

        public void MakeVote(Vote vote)
        {
            try
            {
                vote.Name = playersDict[vote.UserId].Name;
                CurrentRound.AddVote(vote);
            }
            catch (Exception ex)
            {
                Console.WriteLine("MakeVote.ex :" + ex.Message);
            }
        }
        public void MoveToNextTurn()
        {
            try
            {
                CurrentRound.MoveToNextRound();
                presidentIndex = ((++presidentIndex) % AlivePlayers.Count);
                CurrentRound.PresidentId = AlivePlayers[presidentIndex].UserId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("MoveToNexTurn.ex:"+ ex.Message);
            }
        }
    }
}
