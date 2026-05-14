using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.Services
{
    public class TestDataService
    {
        public async Task FillTestDataAsync()
        {
            await FillCountriesAsync();
            await FillChampionshipsAsync();
            await FillGoalTypesAsync();
            await FillPeopleAsync();
            await FillMatchesAsync();
            await FillGoalsAsync();
            await FillPlayerSquadsAsync();
            await FillCoachMatchesAsync();
        }

        private async Task FillCountriesAsync()
        {
            await Task.CompletedTask;
        }

        private async Task FillChampionshipsAsync()
        {
            await Task.CompletedTask;
        }

        private async Task FillGoalTypesAsync()
        {
            await Task.CompletedTask;
        }

        private async Task FillPeopleAsync()
        {
            await Task.CompletedTask;
        }

        private async Task FillMatchesAsync()
        {
            await Task.CompletedTask;
        }

        private async Task FillGoalsAsync()
        {
            await Task.CompletedTask;
        }

        private async Task FillPlayerSquadsAsync()
        {
            await Task.CompletedTask;
        }

        private async Task FillCoachMatchesAsync()
        {
            await Task.CompletedTask;
        }
    }
}
