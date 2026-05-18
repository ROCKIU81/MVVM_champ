using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCupMVVM.Models;
using Npgsql;

namespace WorldCupMVVM.Services
{
    public class CountryRepository : ICountry
    {
        private readonly string _connectionString;

        public CountryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            if (AppSettings.UseTestData)
            {
                return await Task.FromResult(TestDataService.Countries);
            }

            var result = new List<Country>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT id_country, name FROM public.country ORDER BY id_country", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Country
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            return result;
        }

        public async Task<Country> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT id_country, name FROM public.country WHERE id_country = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Country
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task AddAsync(Country country)
        {
            if (AppSettings.UseTestData)
            {
                var maxId = TestDataService.Countries.Count > 0 
                    ? TestDataService.Countries.Max(c => c.Id) 
                    : 0;
                country.Id = maxId + 1;
                TestDataService.Countries.Add(country);
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "INSERT INTO public.country (name) VALUES (@name)", connection))
                {
                    command.Parameters.AddWithValue("@name", country.Name);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Country country)
        {
            if (AppSettings.UseTestData)
            {
                var existing = TestDataService.Countries.FirstOrDefault(c => c.Id == country.Id);
                if (existing != null)
                {
                    existing.Name = country.Name;
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "UPDATE public.country SET name = @name WHERE id_country = @id", connection))
                {
                    command.Parameters.AddWithValue("@name", country.Name);
                    command.Parameters.AddWithValue("@id", country.Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (AppSettings.UseTestData)
            {
                var country = TestDataService.Countries.FirstOrDefault(c => c.Id == id);
                if (country != null)
                {
                    TestDataService.Countries.Remove(country);
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "DELETE FROM public.country WHERE id_country = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

