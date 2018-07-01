using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Curnik.Models.Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Curnik.Api
{
    [Produces("application/json")]
    [Route("api/RoomApi")]
    public class RoomApiController : Controller
    {
        private readonly DatabaseContext _context;
        public RoomApiController(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
        }

        // GET: api/PlayerAmount/5
        [HttpGet]
        public async Task<IActionResult> UserAmount()
        {
            var amount = await _context.Rooms.SumAsync(x => x.UsersAmount);
            return Ok(amount.ToString());
        }
        [Route("getusername")]
        [HttpGet]
        public async Task<IActionResult> GetUsername(int id)
        {
            var username = await (from p in this._context.Users where p.Id == id select p.UserName).FirstOrDefaultAsync();

            if (username != null)
            {
                return Content(username);
            }
            return Content("Error");
        }

        [HttpPost]
        public async Task<IActionResult> DisconnectFromRoom([FromBody]int userId)
        {
            var room = await (from p in this._context.Rooms where p.SecondUserId == userId || p.RoomOwnerId == userId select p).FirstOrDefaultAsync();

            if (room != null)
            {
                if (userId == room.SecondUserId)
                {
                    room.SecondUserId = null;
                    room.UsersAmount--;
                }

                else if (userId == room.RoomOwnerId)
                {
                    this._context.Rooms.Remove(room);
                }

                await this._context.SaveChangesAsync();
                return Content("Success");
            }
            return Content("Error");
        }
        [Route("connecttoroomasync")]
        [HttpPost]
        public async Task<IActionResult> ConnectToRoomAsync([FromBody]Rooms model)
        {
            var room = await (from p in this._context.Rooms where model.RoomId == p.RoomId select p).FirstOrDefaultAsync();
            room.SecondUserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (room.UsersAmount < 2)
            {
                room.UsersAmount++;
            }

            await this._context.SaveChangesAsync();
            return Ok();
        }
        [Route("changegamestateasync")]
        [HttpPut]
        public async Task<IActionResult> ChangeGameStateAsync([FromBody]Rooms model)
        {
            var room = await (from p in this._context.Rooms where p.RoomId == model.RoomId select p).FirstOrDefaultAsync();
            if (room != null)
            {
                room.GameState = model.GameState;
            }
            await this._context.SaveChangesAsync();
            return Ok();
        }
    }
}