using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorldCupMVVM.Models
{
    public interface ICoachMatchRepository
    {
        Task<IEnumerable<CoachMatch>> GetAllAsync();
        Task<CoachMatch> GetByIdAsync(int id);
        Task<IEnumerable<CoachMatch>> GetByMatchAsync(int matchId);
        Task AddAsync(CoachMatch coachMatch);
        Task UpdateAsync(CoachMatch coachMatch);
        Task DeleteAsync(int id);
    }
}
