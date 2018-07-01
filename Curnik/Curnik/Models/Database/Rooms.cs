using System;
using System.Collections.Generic;

namespace Curnik.Models.Database
{
    public partial class Rooms
    {
        public int RoomId { get; set; }
        public int RoomOwnerId { get; set; }
        public int? SecondUserId { get; set; }
        public int UsersAmount { get; set; }
        public int GameType { get; set; }
        public int GameState { get; set; }
    }
}
