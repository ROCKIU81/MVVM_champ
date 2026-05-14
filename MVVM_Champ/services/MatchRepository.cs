using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldCupMVVM.Models;
using Npgsql;

namespace WorldCupMVVM.Services
{
    public class MatchRepository : IMatchRepository
    {
        private readonly string _connectionString;

        public MatchRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Match>> GetAllAsync()
        {
            var result = new List<Match>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT m.id_match, m.championship_id, m.team1_id, m.team2_id, m.team1_score, m.team2_score, c1.name, c2.name, ch.year_played, ch.host_city " +
                    "FROM public.\"match\" m " +
                    "JOIN public.championship ch ON ch.id_championship = m.championship_id " +
                    "JOIN public.country c1 ON c1.id_country = m.team1_id " +
                    "JOIN public.country c2 ON c2.id_country = m.team2_id " +
                    "ORDER BY m.id_match", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Match
                        {
                            Id = reader.GetInt32(0),
                            ChampionshipId = reader.GetInt32(1),
                            Team1Id = reader.GetInt32(2),
                            Team2Id = reader.GetInt32(3),
                            Team1Score = reader.GetInt32(4),
                            Team2Score = reader.GetInt32(5),
                            Championship = new Championship { Id = reader.GetInt32(1), Year = reader.GetInt32(8), City = reader.GetString(9) },
                            Team1 = new Country { Id = reader.GetInt32(2), Name = reader.GetString(6) },
                            Team2 = new Country { Id = reader.GetInt32(3), Name = reader.GetString(7) }
                        });
                    }
                }
            }

            return result;
        }

        public async Task<Match> GetByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT id_match, championship_id, team1_id, team2_id, team1_score, team2_score FROM public.\"match\" WHERE id_match = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Match
                            {
                                Id = reader.GetInt32(0),
                                ChampionshipId = reader.GetInt32(1),
                                Team1Id = reader.GetInt32(2),
                                Team2Id = reader.GetInt32(3),
                                Team1Score = reader.GetInt32(4),
                                Team2Score = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<IEnumerable<Match>> GetByChampionshipAsync(int championshipId)
        {
            var result = new List<Match>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT m.id_match, m.championship_id, m.team1_id, m.team2_id, m.team1_score, m.team2_score, c1.name, c2.name, ch.year_played, ch.host_city " +
                    "FROM public.\"match\" m " +
                    "JOIN public.championship ch ON ch.id_championship = m.championship_id " +
                    "JOIN public.country c1 ON c1.id_country = m.team1_id " +
                    "JOIN public.country c2 ON c2.id_country = m.team2_id " +
                    "WHERE m.championship_id = @champId ORDER BY m.id_match", connection))
                {
                    command.Parameters.AddWithValue("@champId", championshipId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Match
                            {
                                Id = reader.GetInt32(0),
                                ChampionshipId = reader.GetInt32(1),
                                Team1Id = reader.GetInt32(2),
                                Team2Id = reader.GetInt32(3),
                                Team1Score = reader.GetInt32(4),
                                Team2Score = reader.GetInt32(5),
                                Championship = new Championship { Id = reader.GetInt32(1), Year = reader.GetInt32(8), City = reader.GetString(9) },
                                Team1 = new Country { Id = reader.GetInt32(2), Name = reader.GetString(6) },
                                Team2 = new Country { Id = reader.GetInt32(3), Name = reader.GetString(7) }
                            });
                        }
                    }
                }
            }

            return result;
        }

        public async Task AddAsync(Match match)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "INSERT INTO public.\"match\" (championship_id, team1_id, team2_id, team1_score, team2_score) VALUES (@champId, @team1, @team2, @score1, @score2)", connection))
                {
                    command.Parameters.AddWithValue("@champId", match.ChampionshipId);
                    command.Parameters.AddWithValue("@team1", match.Team1Id);
                    command.Parameters.AddWithValue("@team2", match.Team2Id);
                    command.Parameters.AddWithValue("@score1", match.Team1Score);
                    command.Parameters.AddWithValue("@score2", match.Team2Score);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Match match)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "UPDATE public.\"match\" SET championship_id = @champId, team1_id = @team1, team2_id = @team2, team1_score = @score1, team2_score = @score2 WHERE id_match = @id", connection))
                {
                    command.Parameters.AddWithValue("@champId", match.ChampionshipId);
                    command.Parameters.AddWithValue("@team1", match.Team1Id);
                    command.Parameters.AddWithValue("@team2", match.Team2Id);
                    command.Parameters.AddWithValue("@score1", match.Team1Score);
                    command.Parameters.AddWithValue("@score2", match.Team2Score);
                    command.Parameters.AddWithValue("@id", match.Id);
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
                    "DELETE FROM public.\"match\" WHERE id_match = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
