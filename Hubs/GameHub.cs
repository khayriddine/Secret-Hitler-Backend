using Microsoft.AspNetCore.SignalR;
using Secret_Hitler_Backend.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Secret_Hitler_Backend.Hubs
{
    public class GameHub : Hub
    {
        public static Dictionary<int, string> cnxIds;
        public static Dictionary<int, Room> rooms;
        public static Dictionary<int, Game> games;
        public static Dictionary<int, int> players;
        public static int roomsNbre;
        public GameHub()
        {
            if (cnxIds == null)
            {
                cnxIds = new Dictionary<int, string>();
            }
            if(rooms == null)
            {
                rooms = new Dictionary<int, Room>();
                roomsNbre = 0;
            }
            if(games == null)
            {
                games = new Dictionary<int, Game>();
            }
            if (players == null) 
            {
                players = new Dictionary<int, int>();
            }
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public void RegisterUser(int id)
        {
            try
            {
                if (cnxIds != null)
                {
                    cnxIds[id] = Context.ConnectionId;
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine("RegisterUser.ex: "+ ex.Message);
            }
        }
        public Task GetCurrentRound(int roomId)
        {
            try
            {
                if (games.ContainsKey(roomId))
                {
                    return Clients.Caller.SendAsync("CurrentRoundUpdated", games[roomId].CurrentRound);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("RegisterUser.ex: " + ex.Message);
                Clients.All.SendAsync("GetError", "GetCurrentRound.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "GetCurrentRound.ex: Room does not exists");
        }
        public Task GetCurrentGame(int roomId)
        {
            try
            {
                if (games.ContainsKey(roomId))
                {
                    return Clients.Caller.SendAsync("LoadingGame", games[roomId]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetCurrentGame.ex : "+ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "GetCurrentGame.ex: game does not exists");
        }
        public void CreateRoom(Room room)
        {
            try
            {
                lock (rooms)
                {
                    room.RoomId = roomsNbre++;
                    rooms[room.RoomId] = room;
                }
                
            }
            catch (Exception ex)
            {

                Console.WriteLine("CreateRoom.ex: "+ex.Message);
                 Clients.All.SendAsync("GetError", "CreateRoom.ex: " + ex.Message);
            }
             Clients.All.SendAsync("GetRoom", room);
        }
        public Task JoinRoom(int roomId,User user)
        {
            try
            {
                lock (rooms)
                {
                    if (rooms.ContainsKey(roomId))
                    {
                        if(rooms[roomId].Users == null)
                        {
                            rooms[roomId].Users = new List<User>();
                            
                        }
                        var usr = rooms[roomId].Users.FirstOrDefault(u => u.UserId == user.UserId);
                        if (usr != null)
                        {
                            usr = user;
                        }
                        else
                        {
                            rooms[roomId].Users.Add(user);
                        }
                        return Clients.All.SendAsync("GetRoom", rooms[roomId]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("JoinRoom.ex: "+ ex.Message);
                return Clients.All.SendAsync("GetError", "JoinRoom.ex: " + ex.Message);
            }
            return Clients.All.SendAsync("GetInfo", "No room with the specifed Id");
        }

        public Task LeaveRoom(int roomId, int userId)
        {
            try
            {
                lock (rooms)
                {
                    if (rooms.ContainsKey(roomId))
                    {
                        if (rooms[roomId].Users != null)
                        {
                            var usr = rooms[roomId].Users.FirstOrDefault(u => u.UserId == userId);
                            if (usr != null)
                            {
                                rooms[roomId].Users.Remove(usr);
                            }
                            return Clients.All.SendAsync("GetRoom", rooms[roomId]);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("JoinRoom.ex: " + ex.Message);
                return Clients.Caller.SendAsync("GetError", "LeaveRoom.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetInfo", "No room with the specifed Id");
        }
        public Task RemoveRoom(int roomId)
        {
            try
            {
                lock (rooms)
                {
                    if (rooms.ContainsKey(roomId))
                    {
                        rooms.Remove(roomId);
                        return Clients.All.SendAsync("GetRooms", rooms.Values.ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Remove.ex: " + ex.Message);
                return Clients.All.SendAsync("GetError", "RemoveRoom.ex: " + ex.Message);
            }
            return Clients.All.SendAsync("GetInfo", "No room with the specifed Id");
        }
        public Task Play(int roomId)
        {
            List<string> cnxs = new List<string>();
            try
            {
                Room room = rooms[roomId];
                games[room.RoomId] = new Game(room);
                
                foreach(var u in room.Users)
                {
                    players[u.UserId] = room.RoomId;
                    cnxs.Add(cnxIds[u.UserId]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Play.ex: "+ ex.Message);
                return Clients.Caller.SendAsync("GetError", "Play.ex: " + ex.Message);
            }
            return Clients.Clients(cnxs).SendAsync("LoadingGame",games[roomId]);
        }
        public Task NominateChancellor(int prezId,int chancId,int roomId)
        {
            try
            {
                if(games.ContainsKey(roomId))
                {
                    var game = games[roomId];
                    if (game.CanNominateChancellor(prezId, chancId))
                    {
                        game.NominateChancellor(chancId);
                        List<string> cnxs = new List<string>();
                        foreach (var u in game.Players)
                        {
                            cnxs.Add(cnxIds[u.UserId]);
                        }
                        return Clients.Clients(cnxs).SendAsync("CurrentRoundUpdated", game.CurrentRound);
                    }
                    else
                    {
                        return Clients.Caller.SendAsync("GetInfo", "Refresh Browser");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("NominateChancellor.ex: " + ex.Message);
                return Clients.Caller.SendAsync("GetError", "NominateChancellor.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "NominateChancellor.ex: Game does not exists");
        }
        public Task PlayerIsReady(int userId,int roomId)
        {
            try
            {
                var game = games[roomId];
                List<string> cnxs = new List<string>();
                foreach (var u in game.Players)
                {
                    cnxs.Add(cnxIds[u.UserId]);
                }
                if (game != null)
                {
                     var player = game.Players.Find(p => p.UserId == userId);
                    if(player != null){
                        player.Status = Status.Normal;
                    }
                    if(game.Players.Where(p => p.Status == Status.Pending).Count() == 0)
                    {

                        return Clients.Clients(cnxs).SendAsync("RemoveLoading", "true");
                    }
                    else
                    {
                        return Clients.Clients(cnxs).SendAsync("RemoveLoading", "false");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PlayerIsReady.ex: " + ex.Message);
                return Clients.Caller.SendAsync("GetError", "PlayerIsReady.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "PlayerIsReady.ex: game is null");

        }
        public Task VoteOnChancellor(int roomId, Vote vote)
        {
            try
            {

                if (games.ContainsKey(roomId))
                {
                    var game = games[roomId];
                    List<string> cnxs = new List<string>();
                    foreach (var u in game.Players)
                    {
                        cnxs.Add(cnxIds[u.UserId]);
                    }
                    lock (game)
                    {

                        if (game.CanPlayerVote(vote))
                        {
                            game.MakeVote(vote);
                            if (game.AllPalyerVoted == true)
                            {
                                game.CalculateVoteResults(VoteScope.Chancellor);
                                Clients.Clients(cnxs).SendAsync("VoteResult", game.CurrentRound.VoteResults);
                                game.ProceedAfterVote();
                                if(game.GameResult != null)
                                {
                                    games.Remove(roomId);
                                    rooms.Remove(roomId);
                                    return Clients.Clients(cnxs).SendAsync("GameEnd",game);
                                }
                                else
                                {
                                    
                                    if(game.CurrentRound.RoundState == RoundState.PickChancellor)
                                    {
                                        return Clients.Clients(cnxs).SendAsync("NewTurn", game);
                                    }
                                    else
                                    {
                                        return Clients.Clients(cnxs).SendAsync("CurrentRoundUpdated", game.CurrentRound);
                                    }
                                    
                                }
                            }
                            else
                                return Clients.Clients(cnxs).SendAsync("PlayerVoted");
                        }
                        else
                        {
                            return Clients.Clients(cnxs).SendAsync("GetError", "VoteOnChancellor.ex: player cant vote: "+vote.UserId.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("VoteOnChancellor.ex: " + ex.Message);
                return Clients.Caller.SendAsync("GetError", "VoteOnChancellor.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "VoteOnChancellor.ex: game is null");
        }
        public Task VoteOnVeto(int roomId,Vote vote)
        {
            try
            {
                if (games.ContainsKey(roomId))
                {
                    var game = games[roomId];
                    lock (game)
                    {
                        List<string> cnxs = new List<string>();
                        foreach (var u in game.Players)
                        {
                            cnxs.Add(cnxIds[u.UserId]);
                        }

                        if(vote != null)
                        {
                            if(vote.VoteValue == VoteValue.No)
                            {
                                game.CurrentRound.RoundState = RoundState.VetoRefused;
                                return Clients.Clients(cnxs).SendAsync("CurrentRoundUpdated", game.CurrentRound);
                            }
                            else
                            {
                                game.MoveToNextTurn();
                                return Clients.Clients(cnxs).SendAsync("NewTurn", game);
                            }

                        }
                        else
                            return Clients.Caller.SendAsync("GetError", "VoteOnVeto.ex: vote is null");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("VoteOnVeto.ex: " + ex.Message);
                return Clients.Caller.SendAsync("GetError", "VoteOnVeto.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "VoteOnVeto.ex: game is null");
        }
        public Task DiscardCard(int roomId,int discarded)
        {
            try
            {
                if (games.ContainsKey(roomId))
                {
                    var game = games[roomId];
                    lock (game)
                    {
                        List<string> cnxs = new List<string>();
                        foreach (var u in game.Players)
                        {
                            cnxs.Add(cnxIds[u.UserId]);
                        }
                        game.DiscardCard(discarded);
                        if(game.CurrentRound.CardsInHand.Count == 1)
                        {
                            game.MakeActionBeforeEnd();
                        }
                        switch (game.CurrentRound.RoundState)
                        {
                            case RoundState.End:
                                games.Remove(roomId);
                                rooms.Remove(roomId);
                                return Clients.Clients(cnxs).SendAsync("GameEnd", game);
                            case RoundState.PickChancellor :
                                return Clients.Clients(cnxs).SendAsync("NewTurn", game);
                            case RoundState.KillMember:
                                return Clients.Clients(cnxs).SendAsync("NewTurn", game);
                            default:
                                return Clients.Clients(cnxs).SendAsync("CurrentRoundUpdated", game.CurrentRound);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DiscardCard.ex: " + ex.Message);
                return Clients.Caller.SendAsync("GetError", "DiscardCard.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "DiscardCard.ex: game is null");
        }
        public Task InvestigatePlayer(int roomId, int playerId)
        {
            try
            {
                if (games.ContainsKey(roomId))
                {
                    var game = games[roomId];
                    lock (game)
                    {
                        List<string> cnxs = new List<string>();
                        foreach (var u in game.Players)
                        {
                            cnxs.Add(cnxIds[u.UserId]);
                        }
                        var player = game.GetPlayerById(playerId);
                        if(player != null)
                        {
                            game.CurrentRound.RoundState = RoundState.MembershipInvestigated;
                            return Clients.Clients(cnxs).SendAsync("CurrentRoundUpdated", game.CurrentRound);
                        }else
                        {
                            return Clients.Caller.SendAsync("GetError", "PickNextPresident.ex: did not find the player");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PickNextPresident.ex: " + ex.Message);
                return Clients.Caller.SendAsync("GetError", "PickNextPresident.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "PickNextPresident.ex: game is null");
        }
        public Task MoveToNextTurn(int roomId)
        {
            try
            {
                if (games.ContainsKey(roomId))
                {
                    var game = games[roomId];
                    lock (game)
                    {
                        List<string> cnxs = new List<string>();
                        foreach (var u in game.Players)
                        {
                            cnxs.Add(cnxIds[u.UserId]);
                        }
                        game.MoveToNextTurn();
                        return Clients.Clients(cnxs).SendAsync("CurrentRoundUpdated", game.CurrentRound);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PickNextPresident.ex: " + ex.Message);
                return Clients.Caller.SendAsync("GetError", "PickNextPresident.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "PickNextPresident.ex: game is null");
        }
        public Task PickNextPresident(int roomId,int playerId)
        {
            try
            {
                if (games.ContainsKey(roomId))
                {
                    var game = games[roomId];
                    lock (game)
                    {
                        List<string> cnxs = new List<string>();
                        foreach (var u in game.Players)
                        {
                            cnxs.Add(cnxIds[u.UserId]);
                        }
                        var player = game.GetPlayerById(playerId);
                        if (player != null)
                        {
                            game.PickNextPresident(playerId);
                            return Clients.Clients(cnxs).SendAsync("CurrentRoundUpdated", game.CurrentRound);
                        }
                        else
                        {
                            return Clients.Caller.SendAsync("GetError", "PickNextPresident.ex: did not find the player");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PickNextPresident.ex: " + ex.Message);
                return Clients.Caller.SendAsync("GetError", "PickNextPresident.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "PickNextPresident.ex: game is null");
        }
        public Task KillPlayer(int roomId, int playerId)
        {
            try
            {
                if (games.ContainsKey(roomId))
                {
                    var game = games[roomId];
                    lock (game)
                    {
                        List<string> cnxs = new List<string>();
                        foreach (var u in game.Players)
                        {
                            cnxs.Add(cnxIds[u.UserId]);
                        }
                        game.KillPlayer(playerId);
                        if (game.GameResult != null)
                        {
                            games.Remove(roomId);
                            rooms.Remove(roomId);
                            return Clients.Clients(cnxs).SendAsync("GameEnd", game);
                        }
                        else
                        {
                            return Clients.Clients(cnxs).SendAsync("CurrentRoundUpdated", game.CurrentRound);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("KillPlayer.ex: " + ex.Message);
                return Clients.Caller.SendAsync("GetError", "KillPlayer.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "KillPlayer.ex: game is null");
        }
        public Task RequestVeto(int roomId)
        {
            try
            {
                if (games.ContainsKey(roomId))
                {
                    var game = games[roomId];
                    lock (game)
                    {
                        List<string> cnxs = new List<string>();
                        foreach (var u in game.Players)
                        {
                            cnxs.Add(cnxIds[u.UserId]);
                        }
                        game.CurrentRound.RoundState = RoundState.VetoRequested;
                        return Clients.Clients(cnxs).SendAsync("CurrentRoundUpdated", game.CurrentRound);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("RequestVeto.ex: " + ex.Message);
                return Clients.Caller.SendAsync("GetError", "RequestVeto.ex: " + ex.Message);
            }
            return Clients.Caller.SendAsync("GetError", "RequestVeto.ex: game is null");
        }
        public override async Task OnConnectedAsync()
        {

            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("GetRooms", rooms.Values.ToList());
        }

    }
    public class UserComparer : IEqualityComparer<User>
    {
        public bool Equals([AllowNull] User x, [AllowNull] User y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            return x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return (obj.Name + obj.UserId.ToString()).ToLower().GetHashCode();
        }
    }
}
