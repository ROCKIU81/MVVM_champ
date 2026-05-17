using Xunit;
using System;
using WorldCupMVVM.Models;

namespace MVVM_champ_test
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
                DateOfBirth = new DateTime(1990, 5, 15),
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
                DateOfBirth = new DateTime(1963, 9, 2),
                Status = "coach" 
            };

            Assert.Equal("coach", person.Status);
        }

        [Fact]
        public void Person_CanUpdateFullName()
        {
            var person = new Person { Id = 1, FullName = "Иван Петров", Status = "player" };
            person.FullName = "Сергей Иванов";

            Assert.Equal("Сергей Иванов", person.FullName);
        }

        [Fact]
        public void Person_CanUpdateStatus()
        {
            var person = new Person { Id = 1, FullName = "Иван Петров", Status = "player" };
            person.Status = "coach";

            Assert.Equal("coach", person.Status);
        }

        [Fact]
        public void Person_WithDifferentDates_CreatesSuccessfully()
        {
            var person1 = new Person { Id = 1, FullName = "Иван Петров", DateOfBirth = new DateTime(1990, 5, 15), Status = "player" };
            var person2 = new Person { Id = 2, FullName = "Сергей Иванов", DateOfBirth = new DateTime(1985, 3, 20), Status = "player" };

            Assert.NotEqual(person1.DateOfBirth, person2.DateOfBirth);
        }
    }
}
