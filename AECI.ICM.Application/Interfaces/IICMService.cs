using AECI.ICM.Domain.Entities;
using System.Collections.Generic;

namespace AECI.ICM.Domain.Interfaces
{
    public interface IICMService
    {
        IEnumerable<ICMEntity> GetAllAsync();
    }
}
