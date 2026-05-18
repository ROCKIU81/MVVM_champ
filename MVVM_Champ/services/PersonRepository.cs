using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCupMVVM.Models;
using Npgsql;

namespace WorldCupMVVM.Services
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string _connectionString;

        public PersonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            if (AppSettings.UseTestData)
            {
                return await Task.FromResult(TestDataService.People);
            }

            var result = new List<Person>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT id_person, full_name, birth_date, status FROM public.person ORDER BY id_person", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Person
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            DateOfBirth = reader.GetDateTime(2),
                            Status = reader.GetString(3)
                        });
                    }
                }
            }

            return result;
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT id_person, full_name, birth_date, status FROM public.person WHERE id_person = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Person
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                DateOfBirth = reader.GetDateTime(2),
                                Status = reader.GetString(3)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task AddAsync(Person person)
        {
            if (AppSettings.UseTestData)
            {
                var maxId = TestDataService.People.Count > 0 
                    ? TestDataService.People.Max(p => p.Id) 
                    : 0;
                person.Id = maxId + 1;
                TestDataService.People.Add(person);
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "INSERT INTO public.person (full_name, birth_date, status) VALUES (@fio, @dob, @status)", connection))
                {
                    command.Parameters.AddWithValue("@fio", person.FullName);
                    command.Parameters.AddWithValue("@dob", person.DateOfBirth);
                    command.Parameters.AddWithValue("@status", person.Status);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Person person)
        {
            if (AppSettings.UseTestData)
            {
                var existing = TestDataService.People.FirstOrDefault(p => p.Id == person.Id);
                if (existing != null)
                {
                    existing.FullName = person.FullName;
                    existing.DateOfBirth = person.DateOfBirth;
                    existing.Status = person.Status;
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "UPDATE public.person SET full_name = @fio, birth_date = @dob, status = @status WHERE id_person = @id", connection))
                {
                    command.Parameters.AddWithValue("@fio", person.FullName);
                    command.Parameters.AddWithValue("@dob", person.DateOfBirth);
                    command.Parameters.AddWithValue("@status", person.Status);
                    command.Parameters.AddWithValue("@id", person.Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (AppSettings.UseTestData)
            {
                var person = TestDataService.People.FirstOrDefault(p => p.Id == id);
                if (person != null)
                {
                    TestDataService.People.Remove(person);
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "DELETE FROM public.person WHERE id_person = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
