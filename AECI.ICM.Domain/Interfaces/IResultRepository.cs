using AECI.ICM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AECI.ICM.Domain.Interfaces
{
    public interface IResultRepository
    {
        IEnumerable<ResultEntity> GetAllAsync();
    }
}
