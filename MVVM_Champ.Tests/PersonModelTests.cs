using Xunit;
using System.Collections.Generic;
using WorldCupMVVM.Models;

namespace MVVM_Champ.Tests
{
    public class PersonModelTests
    {
        [Fact]
        public void Person_WithPlayerStatus_CreatesSuccessfully()
        {
            var person = new Person 
            { 
                Id = 1, 
                FullName = "Иван Петров", 
                Status = "player" 
            };

            Assert.NotNull(person);
            Assert.Equal("Иван Петров", person.FullName);
            Assert.Equal("player", person.Status);
        }

        [Fact]
        public void Person_WithCoachStatus_CreatesSuccessfully()
        {
            var person = new Person 
            { 
                Id = 2, 
                FullName = "Станислав Черчесов", 
                Status = "coach" 
            };

            Assert.NotNull(person);
            Assert.Equal("coach", person.Status);
        }

        [Fact]
        public void Person_CanAddGoals()
        {
            var person = new Person 
            { 
                Id = 1, 
                FullName = "Иван Петров", 
                Status = "player",
                Goals = new List<Goal>()
            };

            var goal1 = new Goal { Id = 1, MatchId = 1, PlayerId = 1, Minute = 15, GoalTypeId = 1 };
            var goal2 = new Goal { Id = 2, MatchId = 2, PlayerId = 1, Minute = 45, GoalTypeId = 1 };
            var goal3 = new Goal { Id = 3, MatchId = 3, PlayerId = 1, Minute = 70, GoalTypeId = 1 };

            person.Goals.Add(goal1);
            person.Goals.Add(goal2);
            person.Goals.Add(goal3);

            Assert.Equal(3, person.Goals.Count);
        }

        [Fact]
        public void Person_CanAddPlayerSquads()
        {
            var person = new Person 
            { 
                Id = 1, 
                FullName = "Иван Петров", 
                Status = "player",
                PlayerSquads = new List<PlayerSquad>()
            };

            var squad1 = new PlayerSquad { Id = 1, MatchId = 1, PlayerId = 1, TeamId = 1, PlayerNumber = 10 };
            var squad2 = new PlayerSquad { Id = 2, MatchId = 2, PlayerId = 1, TeamId = 1, PlayerNumber = 10 };

            person.PlayerSquads.Add(squad1);
            person.PlayerSquads.Add(squad2);

            Assert.Equal(2, person.PlayerSquads.Count);
        }

        [Fact]
        public void Person_CanAddCoachMatches()
        {
            var person = new Person 
            { 
                Id = 2, 
                FullName = "Станислав Черчесов", 
                Status = "coach",
                CoachMatches = new List<CoachMatch>()
            };

            var coachMatch1 = new CoachMatch { Id = 1, MatchId = 1, CoachId = 2, TeamId = 1 };
            var coachMatch2 = new CoachMatch { Id = 2, MatchId = 2, CoachId = 2, TeamId = 1 };

            person.CoachMatches.Add(coachMatch1);
            person.CoachMatches.Add(coachMatch2);

            Assert.Equal(2, person.CoachMatches.Count);
        }
    }
}
