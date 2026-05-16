using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldCupMVVM.Models;
using Npgsql;

namespace WorldCupMVVM.Services
{
    public class GoalRepository : IGoalRepository
    {
        private readonly string _connectionString;

        public GoalRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Goal>> GetAllAsync()
        {
            if (AppSettings.UseTestData)
            {
                return await Task.FromResult(TestDataService.Goals);
            }

            var result = new List<Goal>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT g.id_goal, g.match_id, g.player_id, g.minute, g.goal_type_id, p.full_name, gt.type_name, m.team1_score, m.team2_score, c1.name, c2.name " +
                    "FROM public.goal g " +
                    "JOIN public.\"match\" m ON m.id_match = g.match_id " +
                    "JOIN public.country c1 ON c1.id_country = m.team1_id " +
                    "JOIN public.country c2 ON c2.id_country = m.team2_id " +
                    "JOIN public.person p ON p.id_person = g.player_id " +
                    "JOIN public.goal_type gt ON gt.id_goal_type = g.goal_type_id " +
                    "ORDER BY g.id_goal", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Goal
                        {
                            Id = reader.GetInt32(0),
                            MatchId = reader.GetInt32(1),
                            PlayerId = reader.GetInt32(2),
                            Minute = reader.GetInt32(3),
                            GoalTypeId = reader.GetInt32(4),
                            Match = new Match
                            {
                                Id = reader.GetInt32(1),
                                Team1Score = reader.GetInt32(7),
                                Team2Score = reader.GetInt32(8),
                                Team1 = new Country { Name = reader.GetString(9) },
                                Team2 = new Country { Name = reader.GetString(10) }
                            },
                            Player = new Person { Id = reader.GetInt32(2), FullName = reader.GetString(5) },
                            GoalType = new GoalType { Id = reader.GetInt32(4), Name = reader.GetString(6) }
                        });
                    }
                }
            }

            return result;
        }

        public async Task<Goal> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT id_goal, match_id, player_id, minute, goal_type_id FROM public.goal WHERE id_goal = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Goal
                            {
                                Id = reader.GetInt32(0),
                                MatchId = reader.GetInt32(1),
                                PlayerId = reader.GetInt32(2),
                                Minute = reader.GetInt32(3),
                                GoalTypeId = reader.GetInt32(4)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<IEnumerable<Goal>> GetByMatchAsync(int matchId)
        {
            var result = new List<Goal>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT g.id_goal, g.match_id, g.player_id, g.minute, g.goal_type_id, p.full_name, gt.type_name, m.team1_score, m.team2_score, c1.name, c2.name " +
                    "FROM public.goal g " +
                    "JOIN public.\"match\" m ON m.id_match = g.match_id " +
                    "JOIN public.country c1 ON c1.id_country = m.team1_id " +
                    "JOIN public.country c2 ON c2.id_country = m.team2_id " +
                    "JOIN public.person p ON p.id_person = g.player_id " +
                    "JOIN public.goal_type gt ON gt.id_goal_type = g.goal_type_id " +
                    "WHERE g.match_id = @matchId ORDER BY g.minute", connection))
                {
                    command.Parameters.AddWithValue("@matchId", matchId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Goal
                            {
                                Id = reader.GetInt32(0),
                                MatchId = reader.GetInt32(1),
                                PlayerId = reader.GetInt32(2),
                                Minute = reader.GetInt32(3),
                                GoalTypeId = reader.GetInt32(4),
                                Match = new Match
                                {
                                    Id = reader.GetInt32(1),
                                    Team1Score = reader.GetInt32(7),
                                    Team2Score = reader.GetInt32(8),
                                    Team1 = new Country { Name = reader.GetString(9) },
                                    Team2 = new Country { Name = reader.GetString(10) }
                                },
                                Player = new Person { Id = reader.GetInt32(2), FullName = reader.GetString(5) },
                                GoalType = new GoalType { Id = reader.GetInt32(4), Name = reader.GetString(6) }
                            });
                        }
                    }
                }
            }

            return result;
        }

        public async Task AddAsync(Goal goal)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "INSERT INTO public.goal (match_id, player_id, minute, goal_type_id) VALUES (@matchId, @playerId, @minute, @goalTypeId)", connection))
                {
                    command.Parameters.AddWithValue("@matchId", goal.MatchId);
                    command.Parameters.AddWithValue("@playerId", goal.PlayerId);
                    command.Parameters.AddWithValue("@minute", goal.Minute);
                    command.Parameters.AddWithValue("@goalTypeId", goal.GoalTypeId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Goal goal)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "UPDATE public.goal SET match_id = @matchId, player_id = @playerId, minute = @minute, goal_type_id = @goalTypeId WHERE id_goal = @id", connection))
                {
                    command.Parameters.AddWithValue("@matchId", goal.MatchId);
                    command.Parameters.AddWithValue("@playerId", goal.PlayerId);
                    command.Parameters.AddWithValue("@minute", goal.Minute);
                    command.Parameters.AddWithValue("@goalTypeId", goal.GoalTypeId);
                    command.Parameters.AddWithValue("@id", goal.Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "DELETE FROM public.goal WHERE id_goal = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
