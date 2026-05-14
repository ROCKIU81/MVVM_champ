using System;

namespace WorldCupMVVM.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Status { get; set; } // "Игрок" или "Тренер"
    }
}
