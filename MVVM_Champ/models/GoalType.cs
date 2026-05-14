using System;

namespace WorldCupMVVM.Models
{
    public class GoalType
    {
        public int Id { get; set; }
        public string Name { get; set; } // "С игры", "Пенальти", "Автогол"
        public string Description { get; set; }
    }
}
