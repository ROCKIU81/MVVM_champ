using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorldCupMVVM.Models
{
    public interface IPlayerSquadRepository
    {
        Task<IEnumerable<PlayerSquad>> GetAllAsync();
        Task<PlayerSquad> GetByIdAsync(int id);
        Task<IEnumerable<PlayerSquad>> GetByMatchAsync(int matchId);
        Task AddAsync(PlayerSquad playerSquad);
        Task UpdateAsync(PlayerSquad playerSquad);
        Task DeleteAsync(int id);
    }
}
