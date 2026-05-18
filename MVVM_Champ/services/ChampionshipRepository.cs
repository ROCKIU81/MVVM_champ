using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCupMVVM.Models;
using Npgsql;

namespace WorldCupMVVM.Services
{
    public class ChampionshipRepository : IChampionshipRepository
    {
        private readonly string _connectionString;

        public ChampionshipRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Championship>> GetAllAsync()
        {
            if (AppSettings.UseTestData)
            {
                return await Task.FromResult(TestDataService.Championships);
            }

            var result = new List<Championship>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT ch.id_championship, ch.year_played, ch.host_country_id, ch.host_city, c.name " +
                    "FROM public.championship ch " +
                    "JOIN public.country c ON c.id_country = ch.host_country_id " +
                    "ORDER BY ch.id_championship", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Championship
                        {
                            Id = reader.GetInt32(0),
                            Year = reader.GetInt32(1),
                            CountryId = reader.GetInt32(2),
                            City = reader.GetString(3),
                            Country = new Country { Id = reader.GetInt32(2), Name = reader.GetString(4) }
                        });
                    }
                }
            }

            return result;
        }

        public async Task<Championship> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT id_championship, year_played, host_country_id, host_city FROM public.championship WHERE id_championship = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Championship
                            {
                                Id = reader.GetInt32(0),
                                Year = reader.GetInt32(1),
                                CountryId = reader.GetInt32(2),
                                City = reader.GetString(3)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task AddAsync(Championship championship)
        {
            if (AppSettings.UseTestData)
            {
                var maxId = TestDataService.Championships.Count > 0 
                    ? TestDataService.Championships.Max(c => c.Id) 
                    : 0;
                championship.Id = maxId + 1;
                championship.Country = TestDataService.Countries.FirstOrDefault(c => c.Id == championship.CountryId);
                TestDataService.Championships.Add(championship);
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "INSERT INTO public.championship (year_played, host_country_id, host_city) VALUES (@year, @country, @city)", connection))
                {
                    command.Parameters.AddWithValue("@year", championship.Year);
                    command.Parameters.AddWithValue("@country", championship.CountryId);
                    command.Parameters.AddWithValue("@city", championship.City);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Championship championship)
        {
            if (AppSettings.UseTestData)
            {
                var existing = TestDataService.Championships.FirstOrDefault(c => c.Id == championship.Id);
                if (existing != null)
                {
                    existing.Year = championship.Year;
                    existing.CountryId = championship.CountryId;
                    existing.City = championship.City;
                    existing.Country = TestDataService.Countries.FirstOrDefault(c => c.Id == championship.CountryId);
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "UPDATE public.championship SET year_played = @year, host_country_id = @country, host_city = @city WHERE id_championship = @id", connection))
                {
                    command.Parameters.AddWithValue("@year", championship.Year);
                    command.Parameters.AddWithValue("@country", championship.CountryId);
                    command.Parameters.AddWithValue("@city", championship.City);
                    command.Parameters.AddWithValue("@id", championship.Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (AppSettings.UseTestData)
            {
                var championship = TestDataService.Championships.FirstOrDefault(c => c.Id == id);
                if (championship != null)
                {
                    TestDataService.Championships.Remove(championship);
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "DELETE FROM public.championship WHERE id_championship = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
