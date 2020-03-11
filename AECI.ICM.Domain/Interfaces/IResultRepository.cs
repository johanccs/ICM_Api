using AECI.ICM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AECI.ICM.Domain.Interfaces
{
    public interface IResultRepository
    {
        Task<IEnumerable<ResultEntity>> GetAllAsync();

        Task<IEnumerable<ResultEntity>> GetAllAsync(string region);

        Task<bool> Authorise(ResultEntity entity);
    }
}
