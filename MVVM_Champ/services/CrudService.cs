using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.Services
{
    public class CrudService
    {
        private readonly ICountry _countryRepository;
        private readonly IChampionshipRepository _championshipRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IGoalRepository _goalRepository;
        private readonly IGoalTypeRepository _goalTypeRepository;
        private readonly IPlayerSquadRepository _playerSquadRepository;
        private readonly ICoachMatchRepository _coachMatchRepository;

        public CrudService(
            ICountry countryRepository,
            IChampionshipRepository championshipRepository,
            IMatchRepository matchRepository,
            IPersonRepository personRepository,
            IGoalRepository goalRepository,
            IGoalTypeRepository goalTypeRepository,
            IPlayerSquadRepository playerSquadRepository,
            ICoachMatchRepository coachMatchRepository)
        {
            _countryRepository = countryRepository;
            _championshipRepository = championshipRepository;
            _matchRepository = matchRepository;
            _personRepository = personRepository;
            _goalRepository = goalRepository;
            _goalTypeRepository = goalTypeRepository;
            _playerSquadRepository = playerSquadRepository;
            _coachMatchRepository = coachMatchRepository;
        }

        // Country operations
        public async Task SaveCountryAsync(Country country)
        {
            if (country.Id == 0)
                await _countryRepository.AddAsync(country);
            else
                await _countryRepository.UpdateAsync(country);
        }

        public async Task DeleteCountryAsync(int id)
        {
            await _countryRepository.DeleteAsync(id);
        }

        // Championship operations
        public async Task SaveChampionshipAsync(Championship championship)
        {
            if (championship.Id == 0)
                await _championshipRepository.AddAsync(championship);
            else
                await _championshipRepository.UpdateAsync(championship);
        }

        public async Task DeleteChampionshipAsync(int id)
        {
            await _championshipRepository.DeleteAsync(id);
        }

        // Match operations
        public async Task SaveMatchAsync(Match match)
        {
            if (match.Id == 0)
                await _matchRepository.AddAsync(match);
            else
                await _matchRepository.UpdateAsync(match);
        }

        public async Task DeleteMatchAsync(int id)
        {
            await _matchRepository.DeleteAsync(id);
        }

        // Person operations
        public async Task SavePersonAsync(Person person)
        {
            if (person.Id == 0)
                await _personRepository.AddAsync(person);
            else
                await _personRepository.UpdateAsync(person);
        }

        public async Task DeletePersonAsync(int id)
        {
            await _personRepository.DeleteAsync(id);
        }

        // Goal operations
        public async Task SaveGoalAsync(Goal goal)
        {
            if (goal.Id == 0)
                await _goalRepository.AddAsync(goal);
            else
                await _goalRepository.UpdateAsync(goal);
        }

        public async Task DeleteGoalAsync(int id)
        {
            await _goalRepository.DeleteAsync(id);
        }

        // GoalType operations
        public async Task SaveGoalTypeAsync(GoalType goalType)
        {
            if (goalType.Id == 0)
                await _goalTypeRepository.AddAsync(goalType);
            else
                await _goalTypeRepository.UpdateAsync(goalType);
        }

        public async Task DeleteGoalTypeAsync(int id)
        {
            await _goalTypeRepository.DeleteAsync(id);
        }

        // PlayerSquad operations
        public async Task SavePlayerSquadAsync(PlayerSquad playerSquad)
        {
            if (playerSquad.Id == 0)
                await _playerSquadRepository.AddAsync(playerSquad);
            else
                await _playerSquadRepository.UpdateAsync(playerSquad);
        }

        public async Task DeletePlayerSquadAsync(int id)
        {
            await _playerSquadRepository.DeleteAsync(id);
        }

        // CoachMatch operations
        public async Task SaveCoachMatchAsync(CoachMatch coachMatch)
        {
            if (coachMatch.Id == 0)
                await _coachMatchRepository.AddAsync(coachMatch);
            else
                await _coachMatchRepository.UpdateAsync(coachMatch);
        }

        public async Task DeleteCoachMatchAsync(int id)
        {
            await _coachMatchRepository.DeleteAsync(id);
        }
    }
}
