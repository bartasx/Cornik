using Curnik.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Curnik.Controllers.Games
{
    [Authorize]
    public class GameController : Controller
    {
        public IActionResult TicTacToe(Rooms model)
        {
            return View(model);
        }

        public IActionResult Rps(Rooms model)
        {
            return View(model);
        }
    }
}