using AECI.ICM.Domain.Entities;
using System.Collections.Generic;

namespace AECI.ICM.Application.Interfaces
{
    public interface IResultService
    {
        IEnumerable<ResultEntity> GetAllAsync();
    }
}
