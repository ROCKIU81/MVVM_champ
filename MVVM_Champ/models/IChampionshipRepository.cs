using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorldCupMVVM.Models
{
    public interface IChampionshipRepository
    {
        Task<IEnumerable<Championship>> GetAllAsync();
        Task<Championship> GetByIdAsync(int id);
        Task AddAsync(Championship championship);
        Task UpdateAsync(Championship championship);
        Task DeleteAsync(int id);
    }
}
