using System;

namespace WorldCupMVVM.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Minute { get; set; }
        public int GoalTypeId { get; set; }
        
        public Match Match { get; set; }
        public Person Player { get; set; }
        public GoalType GoalType { get; set; }

        public string PlayerName => Player?.FullName ?? $"Игрок #{PlayerId}";
        public string GoalTypeName => GoalType?.Name ?? $"Тип #{GoalTypeId}";
        public string GoalDisplay => $"{PlayerName}, {Minute}' ({GoalTypeName})";
    }
}
