using System;
using System.Collections.Generic;

namespace WorldCupMVVM.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int ChampionshipId { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        
        public Championship Championship { get; set; }
        public Country Team1 { get; set; }
        public Country Team2 { get; set; }


        public string Team1Name => Team1?.Name ?? $"Команда #{Team1Id}";
        public string Team2Name => Team2?.Name ?? $"Команда #{Team2Id}";
        public string MatchDisplay => $"{Team1Name} {Team1Score}:{Team2Score} {Team2Name}";
    }
}
