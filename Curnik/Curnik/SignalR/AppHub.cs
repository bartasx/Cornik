using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http;
using System.Text;
using Curnik.Enums;

namespace Curnik.SignalR
{
    public class AppHub : Hub
    {
        public async void SendMessage(string message)
        {
            var httpcontext = Context.GetHttpContext();
            var groupName = httpcontext.Request.Query["roomId"];
            await this.Clients.Group(groupName).SendAsync("message", message, await this.GetUsernameAsync());
        }

        public async void ChangeGameStateAsync(int gameState)
        {
            var httpcontext = Context.GetHttpContext();
            var groupName = httpcontext.Request.Query["roomId"];

            switch (gameState)
            {
                case (int)GameState.WaitingForPlayer:
                    await this.Clients.Group(groupName).SendAsync("changeGameState", gameState);
                    await this.Clients.Group(groupName).SendAsync("message",
                        "Oczekiwanie na gracza", "SERWER:");
                    break;


                case (int)GameState.InProggress:

                    await this.Clients.Group(groupName).SendAsync("changeGameState", gameState);

                    if (this.IsRoomOwnerStart())
                    {
                        await this.Clients.OthersInGroup(groupName).SendAsync("sendData", true, 0);
                        await this.Clients.Group(groupName).SendAsync("message",
                            "Gra rozpoczęta! Zaczyna Właściciel stołu", "SERWER:");
                    }
                    else
                    {
                        await this.Clients.Caller.SendAsync("sendData", true, 0);
                        await this.Clients.Group(groupName).SendAsync("message",
                            "Gra rozpoczęta! Zaczyna Gracz drugi!", "SERWER:");
                    }

                    break;

                case (int)GameState.Finished:
                    await this.Clients.Group(groupName).SendAsync("changeGameState", gameState);
                    await this.Clients.Group(groupName).SendAsync("message",
                        "Gra zakończona! Aby zagrać jeszcze raz wciśnij przycisk", "SERWER:");
                    break;



                case (int)GameState.LostConnection:
                    await this.Clients.Group(groupName).SendAsync("changeGameState", gameState);
                    await this.Clients.Group(groupName).SendAsync("message",
                        "Użytkownik stracił połączenie z serwerem", "SERWER:");
                    break;
            }
        }

        public async void SendGameData(bool isFirstTurn, int markedField)
        {
            var httpContext = Context.GetHttpContext();
            var groupName = httpContext.Request.Query["roomId"];

            await this.Clients.OthersInGroup(groupName).SendAsync("sendData", isFirstTurn, markedField);
        }

        public override async Task OnConnectedAsync()
        {
            var httpcontext = Context.GetHttpContext();
            var roomId = httpcontext.Request.Query["roomId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await this.Clients.Group(roomId).SendAsync("userJoin", $"{await this.GetUsernameAsync()} Dołączył do pokoju");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpcontext = Context.GetHttpContext();
            var roomId = httpcontext.Request.Query["roomId"];
            var userId = httpcontext.Request.Query["userId"].ToString();

            using (var client = new HttpClient())
            {
                var uri = $"http://localhost:58128/api/RoomApi";
                client.BaseAddress = new Uri(uri);

                var httpContent = new StringContent(userId, Encoding.UTF8, "text/json");

                var response = await client.PostAsync(uri, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    await this.Clients.Group(roomId).SendAsync("userQuit", $"{await this.GetUsernameAsync()} Wyszedł z pokoju");

                    this.ChangeGameStateAsync(0);

                    await base.OnDisconnectedAsync(exception);
                }
            }
        }

        private async Task<string> GetUsernameAsync()
        {
            using (var client = new HttpClient())
            {
                var httpcontext = Context.GetHttpContext();
                var userId = httpcontext.Request.Query["userId"];
                var uri = $"http://localhost:58128/api/RoomApi/getusername?id={userId}";
                client.BaseAddress = new Uri(uri);
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                return "Undefined";
            }
        }

        private bool IsRoomOwnerStart()
        {
            var random = new Random().Next(0, 100);
            if (random > 50)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}