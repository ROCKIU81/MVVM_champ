using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCupMVVM.Models;
using Npgsql;

namespace WorldCupMVVM.Services
{
    public class GoalTypeRepository : IGoalTypeRepository
    {
        private readonly string _connectionString;

        public GoalTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<GoalType>> GetAllAsync()
        {
            if (AppSettings.UseTestData)
            {
                return await Task.FromResult(TestDataService.GoalTypes);
            }

            var result = new List<GoalType>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT id_goal_type, type_name, description FROM public.goal_type ORDER BY id_goal_type", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new GoalType
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                        });
                    }
                }
            }

            return result;
        }

        public async Task<GoalType> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT id_goal_type, type_name, description FROM public.goal_type WHERE id_goal_type = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new GoalType
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task AddAsync(GoalType goalType)
        {
            if (AppSettings.UseTestData)
            {
                var maxId = TestDataService.GoalTypes.Count > 0 
                    ? TestDataService.GoalTypes.Max(g => g.Id) 
                    : 0;
                goalType.Id = maxId + 1;
                TestDataService.GoalTypes.Add(goalType);
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "INSERT INTO public.goal_type (type_name, description) VALUES (@name, @description)", connection))
                {
                    command.Parameters.AddWithValue("@name", goalType.Name);
                    command.Parameters.AddWithValue("@description", goalType.Description ?? (object)DBNull.Value);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(GoalType goalType)
        {
            if (AppSettings.UseTestData)
            {
                var existing = TestDataService.GoalTypes.FirstOrDefault(g => g.Id == goalType.Id);
                if (existing != null)
                {
                    existing.Name = goalType.Name;
                    existing.Description = goalType.Description;
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "UPDATE public.goal_type SET type_name = @name, description = @description WHERE id_goal_type = @id", connection))
                {
                    command.Parameters.AddWithValue("@name", goalType.Name);
                    command.Parameters.AddWithValue("@description", goalType.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@id", goalType.Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (AppSettings.UseTestData)
            {
                var goalType = TestDataService.GoalTypes.FirstOrDefault(g => g.Id == id);
                if (goalType != null)
                {
                    TestDataService.GoalTypes.Remove(goalType);
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "DELETE FROM public.goal_type WHERE id_goal_type = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
