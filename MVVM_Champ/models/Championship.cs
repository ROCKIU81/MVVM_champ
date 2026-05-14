using System;

namespace WorldCupMVVM.Models
{
    public class Championship
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int CountryId { get; set; }
        public string City { get; set; }
        public Country Country { get; set; }

        public string HostDisplay => Country?.Name ?? $"Страна #{CountryId}";
        public string ChampionshipDisplay => $"{Year} - {City}";
    }
}
