using System;

namespace WorldCupMVVM.Models
{
    public class CoachMatch
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int CoachId { get; set; }
        public int TeamId { get; set; }
        
        public Match Match { get; set; }
        public Person Coach { get; set; }
        public Country Team { get; set; }

        public string MatchName => Match?.MatchDisplay ?? $"Матч #{MatchId}";
        public string CoachName => Coach?.FullName ?? $"Тренер #{CoachId}";
        public string TeamName => Team?.Name ?? $"Команда #{TeamId}";
        public string CoachDisplay => $"{CoachName} ({TeamName})";
    }
}
