using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorldCupMVVM.Models
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetAllAsync();
        Task<Match> GetByIdAsync(int id);
        Task<IEnumerable<Match>> GetByChampionshipAsync(int championshipId);
        Task AddAsync(Match match);
        Task UpdateAsync(Match match);
        Task DeleteAsync(int id);
    }
}
