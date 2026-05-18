using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCupMVVM.Models;
using Npgsql;

namespace WorldCupMVVM.Services
{
    public class CoachMatchRepository : ICoachMatchRepository
    {
        private readonly string _connectionString;

        public CoachMatchRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<CoachMatch>> GetAllAsync()
        {
            if (AppSettings.UseTestData)
            {
                return await Task.FromResult(TestDataService.CoachMatches);
            }

            var result = new List<CoachMatch>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT cm.id_record, cm.match_id, cm.coach_id, cm.team_id, p.full_name, c.name, m.team1_score, m.team2_score, c1.name, c2.name " +
                    "FROM public.coach_match cm " +
                    "JOIN public.\"match\" m ON m.id_match = cm.match_id " +
                    "JOIN public.country c1 ON c1.id_country = m.team1_id " +
                    "JOIN public.country c2 ON c2.id_country = m.team2_id " +
                    "JOIN public.person p ON p.id_person = cm.coach_id " +
                    "JOIN public.country c ON c.id_country = cm.team_id " +
                    "ORDER BY cm.id_record", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new CoachMatch
                        {
                            Id = reader.GetInt32(0),
                            MatchId = reader.GetInt32(1),
                            CoachId = reader.GetInt32(2),
                            TeamId = reader.GetInt32(3),
                            Match = new Match
                            {
                                Id = reader.GetInt32(1),
                                Team1Score = reader.GetInt32(6),
                                Team2Score = reader.GetInt32(7),
                                Team1 = new Country { Name = reader.GetString(8) },
                                Team2 = new Country { Name = reader.GetString(9) }
                            },
                            Coach = new Person { Id = reader.GetInt32(2), FullName = reader.GetString(4) },
                            Team = new Country { Id = reader.GetInt32(3), Name = reader.GetString(5) }
                        });
                    }
                }
            }

            return result;
        }

        public async Task<CoachMatch> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT id_record, match_id, coach_id, team_id FROM public.coach_match WHERE id_record = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new CoachMatch
                            {
                                Id = reader.GetInt32(0),
                                MatchId = reader.GetInt32(1),
                                CoachId = reader.GetInt32(2),
                                TeamId = reader.GetInt32(3)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<IEnumerable<CoachMatch>> GetByMatchAsync(int matchId)
        {
            var result = new List<CoachMatch>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT cm.id_record, cm.match_id, cm.coach_id, cm.team_id, p.full_name, c.name, m.team1_score, m.team2_score, c1.name, c2.name " +
                    "FROM public.coach_match cm " +
                    "JOIN public.\"match\" m ON m.id_match = cm.match_id " +
                    "JOIN public.country c1 ON c1.id_country = m.team1_id " +
                    "JOIN public.country c2 ON c2.id_country = m.team2_id " +
                    "JOIN public.person p ON p.id_person = cm.coach_id " +
                    "JOIN public.country c ON c.id_country = cm.team_id " +
                    "WHERE cm.match_id = @matchId ORDER BY cm.id_record", connection))
                {
                    command.Parameters.AddWithValue("@matchId", matchId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new CoachMatch
                            {
                                Id = reader.GetInt32(0),
                                MatchId = reader.GetInt32(1),
                                CoachId = reader.GetInt32(2),
                                TeamId = reader.GetInt32(3),
                                Match = new Match
                                {
                                    Id = reader.GetInt32(1),
                                    Team1Score = reader.GetInt32(6),
                                    Team2Score = reader.GetInt32(7),
                                    Team1 = new Country { Name = reader.GetString(8) },
                                    Team2 = new Country { Name = reader.GetString(9) }
                                },
                                Coach = new Person { Id = reader.GetInt32(2), FullName = reader.GetString(4) },
                                Team = new Country { Id = reader.GetInt32(3), Name = reader.GetString(5) }
                            });
                        }
                    }
                }
            }

            return result;
        }

        public async Task AddAsync(CoachMatch coachMatch)
        {
            if (AppSettings.UseTestData)
            {
                var maxId = TestDataService.CoachMatches.Count > 0 
                    ? TestDataService.CoachMatches.Max(cm => cm.Id) 
                    : 0;
                coachMatch.Id = maxId + 1;
                coachMatch.Match = TestDataService.Matches.FirstOrDefault(m => m.Id == coachMatch.MatchId);
                coachMatch.Coach = TestDataService.People.FirstOrDefault(p => p.Id == coachMatch.CoachId);
                coachMatch.Team = TestDataService.Countries.FirstOrDefault(c => c.Id == coachMatch.TeamId);
                TestDataService.CoachMatches.Add(coachMatch);
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "INSERT INTO public.coach_match (match_id, coach_id, team_id) VALUES (@matchId, @coachId, @teamId)", connection))
                {
                    command.Parameters.AddWithValue("@matchId", coachMatch.MatchId);
                    command.Parameters.AddWithValue("@coachId", coachMatch.CoachId);
                    command.Parameters.AddWithValue("@teamId", coachMatch.TeamId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(CoachMatch coachMatch)
        {
            if (AppSettings.UseTestData)
            {
                var existing = TestDataService.CoachMatches.FirstOrDefault(cm => cm.Id == coachMatch.Id);
                if (existing != null)
                {
                    existing.MatchId = coachMatch.MatchId;
                    existing.CoachId = coachMatch.CoachId;
                    existing.TeamId = coachMatch.TeamId;
                    existing.Match = TestDataService.Matches.FirstOrDefault(m => m.Id == coachMatch.MatchId);
                    existing.Coach = TestDataService.People.FirstOrDefault(p => p.Id == coachMatch.CoachId);
                    existing.Team = TestDataService.Countries.FirstOrDefault(c => c.Id == coachMatch.TeamId);
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "UPDATE public.coach_match SET match_id = @matchId, coach_id = @coachId, team_id = @teamId WHERE id_record = @id", connection))
                {
                    command.Parameters.AddWithValue("@matchId", coachMatch.MatchId);
                    command.Parameters.AddWithValue("@coachId", coachMatch.CoachId);
                    command.Parameters.AddWithValue("@teamId", coachMatch.TeamId);
                    command.Parameters.AddWithValue("@id", coachMatch.Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (AppSettings.UseTestData)
            {
                var coachMatch = TestDataService.CoachMatches.FirstOrDefault(cm => cm.Id == id);
                if (coachMatch != null)
                {
                    TestDataService.CoachMatches.Remove(coachMatch);
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "DELETE FROM public.coach_match WHERE id_record = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
