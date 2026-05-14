using System;

namespace WorldCupMVVM.Models
{
    public class PlayerSquad
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public int PlayerNumber { get; set; }
        
        public Match Match { get; set; }
        public Person Player { get; set; }
        public Country Team { get; set; }

        public string MatchName => Match?.MatchDisplay ?? $"Матч #{MatchId}";
        public string PlayerName => Player?.FullName ?? $"Игрок #{PlayerId}";
        public string TeamName => Team?.Name ?? $"Команда #{TeamId}";
        public string SquadDisplay => $"#{PlayerNumber} {PlayerName} ({TeamName})";
    }
}
