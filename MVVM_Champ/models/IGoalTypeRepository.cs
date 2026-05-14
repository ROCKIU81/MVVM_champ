using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorldCupMVVM.Models
{
    public interface IGoalTypeRepository
    {
        Task<IEnumerable<GoalType>> GetAllAsync();
        Task<GoalType> GetByIdAsync(int id);
        Task AddAsync(GoalType goalType);
        Task UpdateAsync(GoalType goalType);
        Task DeleteAsync(int id);
    }
}
