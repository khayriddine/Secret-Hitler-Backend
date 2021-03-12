using Secret_Hitler_Backend.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Models
{
    public class Game
    {
        private Dictionary<int, Player> playersDict = new Dictionary<int, Player>();
        public int RoomId { get; set; }
        public List<Player> Players { get { return playersDict.Values.ToList(); } }
        public GameRound CurrentRound { get; set; }
        public List<Card> CardsInHand { get; set; }
        public List<Card> RemainingCards { get; set; }
        public List<Card> DiscardedCards { get; set; }
        public List<Card> CardsOnBoard { get; set; }


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
            CardsInHand = new List<Card>();
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
            CurrentRound.RoundState = RoundState.PickPresident;
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
    }
}
