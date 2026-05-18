using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCupMVVM.Models;
using Npgsql;

namespace WorldCupMVVM.Services
{
    public class PlayerSquadRepository : IPlayerSquadRepository
    {
        private readonly string _connectionString;

        public PlayerSquadRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<PlayerSquad>> GetAllAsync()
        {
            if (AppSettings.UseTestData)
            {
                return await Task.FromResult(TestDataService.PlayerSquads);
            }

            var result = new List<PlayerSquad>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT ps.id_record, ps.match_id, ps.player_id, ps.team_id, ps.player_number, p.full_name, c.name, m.team1_score, m.team2_score, c1.name, c2.name " +
                    "FROM public.player_squad ps " +
                    "JOIN public.\"match\" m ON m.id_match = ps.match_id " +
                    "JOIN public.country c1 ON c1.id_country = m.team1_id " +
                    "JOIN public.country c2 ON c2.id_country = m.team2_id " +
                    "JOIN public.person p ON p.id_person = ps.player_id " +
                    "JOIN public.country c ON c.id_country = ps.team_id " +
                    "ORDER BY ps.id_record", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new PlayerSquad
                        {
                            Id = reader.GetInt32(0),
                            MatchId = reader.GetInt32(1),
                            PlayerId = reader.GetInt32(2),
                            TeamId = reader.GetInt32(3),
                            PlayerNumber = reader.GetInt32(4),
                            Match = new Match
                            {
                                Id = reader.GetInt32(1),
                                Team1Score = reader.GetInt32(7),
                                Team2Score = reader.GetInt32(8),
                                Team1 = new Country { Name = reader.GetString(9) },
                                Team2 = new Country { Name = reader.GetString(10) }
                            },
                            Player = new Person { Id = reader.GetInt32(2), FullName = reader.GetString(5) },
                            Team = new Country { Id = reader.GetInt32(3), Name = reader.GetString(6) }
                        });
                    }
                }
            }

            return result;
        }

        public async Task<PlayerSquad> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT id_record, match_id, player_id, team_id, player_number FROM public.player_squad WHERE id_record = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new PlayerSquad
                            {
                                Id = reader.GetInt32(0),
                                MatchId = reader.GetInt32(1),
                                PlayerId = reader.GetInt32(2),
                                TeamId = reader.GetInt32(3),
                                PlayerNumber = reader.GetInt32(4)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<IEnumerable<PlayerSquad>> GetByMatchAsync(int matchId)
        {
            var result = new List<PlayerSquad>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT ps.id_record, ps.match_id, ps.player_id, ps.team_id, ps.player_number, p.full_name, c.name, m.team1_score, m.team2_score, c1.name, c2.name " +
                    "FROM public.player_squad ps " +
                    "JOIN public.\"match\" m ON m.id_match = ps.match_id " +
                    "JOIN public.country c1 ON c1.id_country = m.team1_id " +
                    "JOIN public.country c2 ON c2.id_country = m.team2_id " +
                    "JOIN public.person p ON p.id_person = ps.player_id " +
                    "JOIN public.country c ON c.id_country = ps.team_id " +
                    "WHERE ps.match_id = @matchId ORDER BY ps.player_number", connection))
                {
                    command.Parameters.AddWithValue("@matchId", matchId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new PlayerSquad
                            {
                                Id = reader.GetInt32(0),
                                MatchId = reader.GetInt32(1),
                                PlayerId = reader.GetInt32(2),
                                TeamId = reader.GetInt32(3),
                                PlayerNumber = reader.GetInt32(4),
                                Match = new Match
                                {
                                    Id = reader.GetInt32(1),
                                    Team1Score = reader.GetInt32(7),
                                    Team2Score = reader.GetInt32(8),
                                    Team1 = new Country { Name = reader.GetString(9) },
                                    Team2 = new Country { Name = reader.GetString(10) }
                                },
                                Player = new Person { Id = reader.GetInt32(2), FullName = reader.GetString(5) },
                                Team = new Country { Id = reader.GetInt32(3), Name = reader.GetString(6) }
                            });
                        }
                    }
                }
            }

            return result;
        }

        public async Task AddAsync(PlayerSquad playerSquad)
        {
            if (AppSettings.UseTestData)
            {
                var maxId = TestDataService.PlayerSquads.Count > 0 
                    ? TestDataService.PlayerSquads.Max(ps => ps.Id) 
                    : 0;
                playerSquad.Id = maxId + 1;
                playerSquad.Match = TestDataService.Matches.FirstOrDefault(m => m.Id == playerSquad.MatchId);
                playerSquad.Player = TestDataService.People.FirstOrDefault(p => p.Id == playerSquad.PlayerId);
                playerSquad.Team = TestDataService.Countries.FirstOrDefault(c => c.Id == playerSquad.TeamId);
                TestDataService.PlayerSquads.Add(playerSquad);
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "INSERT INTO public.player_squad (match_id, player_id, team_id, player_number) VALUES (@matchId, @playerId, @teamId, @number)", connection))
                {
                    command.Parameters.AddWithValue("@matchId", playerSquad.MatchId);
                    command.Parameters.AddWithValue("@playerId", playerSquad.PlayerId);
                    command.Parameters.AddWithValue("@teamId", playerSquad.TeamId);
                    command.Parameters.AddWithValue("@number", playerSquad.PlayerNumber);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(PlayerSquad playerSquad)
        {
            if (AppSettings.UseTestData)
            {
                var existing = TestDataService.PlayerSquads.FirstOrDefault(ps => ps.Id == playerSquad.Id);
                if (existing != null)
                {
                    existing.MatchId = playerSquad.MatchId;
                    existing.PlayerId = playerSquad.PlayerId;
                    existing.TeamId = playerSquad.TeamId;
                    existing.PlayerNumber = playerSquad.PlayerNumber;
                    existing.Match = TestDataService.Matches.FirstOrDefault(m => m.Id == playerSquad.MatchId);
                    existing.Player = TestDataService.People.FirstOrDefault(p => p.Id == playerSquad.PlayerId);
                    existing.Team = TestDataService.Countries.FirstOrDefault(c => c.Id == playerSquad.TeamId);
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "UPDATE public.player_squad SET match_id = @matchId, player_id = @playerId, team_id = @teamId, player_number = @number WHERE id_record = @id", connection))
                {
                    command.Parameters.AddWithValue("@matchId", playerSquad.MatchId);
                    command.Parameters.AddWithValue("@playerId", playerSquad.PlayerId);
                    command.Parameters.AddWithValue("@teamId", playerSquad.TeamId);
                    command.Parameters.AddWithValue("@number", playerSquad.PlayerNumber);
                    command.Parameters.AddWithValue("@id", playerSquad.Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (AppSettings.UseTestData)
            {
                var playerSquad = TestDataService.PlayerSquads.FirstOrDefault(ps => ps.Id == id);
                if (playerSquad != null)
                {
                    TestDataService.PlayerSquads.Remove(playerSquad);
                }
                await Task.CompletedTask;
                return;
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "DELETE FROM public.player_squad WHERE id_record = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
