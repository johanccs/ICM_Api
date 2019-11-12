using AECI.ICM.Domain.Entities;
using System.Collections.Generic;

namespace AECI.ICM.Domain.Interfaces
{
    public interface IICMRepository
    {
        IEnumerable<ICMEntity> GetAllAsync();
    }
}
