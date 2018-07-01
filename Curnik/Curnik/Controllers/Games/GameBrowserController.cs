using System.Linq;
using System.Threading.Tasks;
using Curnik.Enums;
using Curnik.Models.Database;
using Curnik.Models.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Curnik.Controllers.Games
{
    [Authorize]
    public class GameBrowserController : Controller
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public GameBrowserController(DatabaseContext databaseContext, UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
            this._dbContext = databaseContext;
        }

        public IActionResult TicTacToe()
        {
            return View(_dbContext.Rooms.Where(room => room.GameType == (int)GameType.TicTacToe).ToListAsync().Result);
        }

        public IActionResult Rps()
        {
            return View(_dbContext.Rooms.Where(room => room.GameType == (int)GameType.Rps).ToListAsync().Result);
        }

        public async Task<IActionResult> CreateRoom(int gameType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (!this._dbContext.Rooms.Any(x => x.RoomOwnerId == user.Id))
            {
                var room = new Rooms() { RoomOwnerId = user.Id, UsersAmount = 1 };

                switch (gameType)
                {
                    case (int)GameType.TicTacToe:
                        room.GameType = (int)GameType.TicTacToe;
                        break;
                    case (int)GameType.Rps:
                        room.GameType = (int)GameType.Rps;
                        break;
                }

                room.GameState = (int)GameState.WaitingForPlayer;
                await this._dbContext.AddAsync(room);
                await this._dbContext.SaveChangesAsync();

                if (gameType == 0)
                {
                    return RedirectToAction("TicTacToe", "Game", room);
                }
                else 
                {
                    return RedirectToAction("Rps", "Game", room);
                }
            }
            else
            {
                return Ok("Zalozyles juz pokoj!");
            }
        }
    }
}