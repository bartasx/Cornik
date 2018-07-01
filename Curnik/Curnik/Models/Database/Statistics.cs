using System;
using System.Collections.Generic;

namespace Curnik.Models.Database
{
    public partial class Statistics
    {
        public int UserId { get; set; }
        public int? LostGames { get; set; }
        public int? PlayedGamesInTotal { get; set; }
        public int? RankPoints { get; set; }
        public int? WonGames { get; set; }
    }
}
