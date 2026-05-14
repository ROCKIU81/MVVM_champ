using System;
using Microsoft.Extensions.DependencyInjection;
using WorldCupMVVM.Models;
using WorldCupMVVM.Services;
using WorldCupMVVM.ViewModels;

namespace WorldCupMVVM
{
    public class ServiceContainer
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceContainer(string connectionString)
        {
            var services = new ServiceCollection();

           
            services.AddSingleton<ICountry>(new CountryRepository(connectionString));
            services.AddSingleton<IChampionshipRepository>(new ChampionshipRepository(connectionString));
            services.AddSingleton<IMatchRepository>(new MatchRepository(connectionString));
            services.AddSingleton<IPersonRepository>(new PersonRepository(connectionString));
            services.AddSingleton<IGoalRepository>(new GoalRepository(connectionString));
            services.AddSingleton<IGoalTypeRepository>(new GoalTypeRepository(connectionString));
            services.AddSingleton<IPlayerSquadRepository>(new PlayerSquadRepository(connectionString));
            services.AddSingleton<ICoachMatchRepository>(new CoachMatchRepository(connectionString));

          
            services.AddSingleton<CountryViewModel>(sp => 
                new CountryViewModel(sp.GetRequiredService<ICountry>()));
            services.AddSingleton<ChampionshipViewModel>(sp => 
                new ChampionshipViewModel(sp.GetRequiredService<IChampionshipRepository>(), sp.GetRequiredService<ICountry>()));
            services.AddSingleton<MatchViewModel>(sp => 
                new MatchViewModel(sp.GetRequiredService<IMatchRepository>(), sp.GetRequiredService<IChampionshipRepository>(), sp.GetRequiredService<ICountry>()));
            services.AddSingleton<PersonViewModel>(sp => 
                new PersonViewModel(sp.GetRequiredService<IPersonRepository>()));
            services.AddSingleton<GoalViewModel>(sp => 
                new GoalViewModel(sp.GetRequiredService<IGoalRepository>(), sp.GetRequiredService<IMatchRepository>(), sp.GetRequiredService<IPersonRepository>(), sp.GetRequiredService<IGoalTypeRepository>()));
            services.AddSingleton<GoalTypeViewModel>(sp => 
                new GoalTypeViewModel(sp.GetRequiredService<IGoalTypeRepository>()));
            services.AddSingleton<PlayerSquadViewModel>(sp => 
                new PlayerSquadViewModel(sp.GetRequiredService<IPlayerSquadRepository>(), sp.GetRequiredService<IMatchRepository>(), sp.GetRequiredService<IPersonRepository>(), sp.GetRequiredService<ICountry>()));
            services.AddSingleton<CoachMatchViewModel>(sp => 
                new CoachMatchViewModel(sp.GetRequiredService<ICoachMatchRepository>(), sp.GetRequiredService<IMatchRepository>(), sp.GetRequiredService<IPersonRepository>(), sp.GetRequiredService<ICountry>()));
            
            
            services.AddSingleton<CrudService>(sp =>
                new CrudService(
                    sp.GetRequiredService<ICountry>(),
                    sp.GetRequiredService<IChampionshipRepository>(),
                    sp.GetRequiredService<IMatchRepository>(),
                    sp.GetRequiredService<IPersonRepository>(),
                    sp.GetRequiredService<IGoalRepository>(),
                    sp.GetRequiredService<IGoalTypeRepository>(),
                    sp.GetRequiredService<IPlayerSquadRepository>(),
                    sp.GetRequiredService<ICoachMatchRepository>()));

            services.AddSingleton<StatisticsService>(sp =>
                new StatisticsService(
                    sp.GetRequiredService<IGoalRepository>(),
                    sp.GetRequiredService<IMatchRepository>(),
                    sp.GetRequiredService<IChampionshipRepository>(),
                    sp.GetRequiredService<IGoalTypeRepository>()));

            services.AddSingleton<StatisticsViewModel>(sp =>
                new StatisticsViewModel(sp.GetRequiredService<StatisticsService>()));

           
            services.AddSingleton<MainViewModel>(sp =>
                new MainViewModel(
                    sp.GetRequiredService<CountryViewModel>(),
                    sp.GetRequiredService<ChampionshipViewModel>(),
                    sp.GetRequiredService<MatchViewModel>(),
                    sp.GetRequiredService<PersonViewModel>(),
                    sp.GetRequiredService<GoalViewModel>(),
                    sp.GetRequiredService<GoalTypeViewModel>(),
                    sp.GetRequiredService<PlayerSquadViewModel>(),
                    sp.GetRequiredService<CoachMatchViewModel>(),
                    sp.GetRequiredService<StatisticsViewModel>()));

            _serviceProvider = services.BuildServiceProvider();
        }

        public T GetService<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}


