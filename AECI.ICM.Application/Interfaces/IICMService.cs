using AECI.ICM.Shared.ViewModels;
using System.Collections.Generic;

namespace AECI.ICM.Domain.Interfaces
{
    public interface IICMService
    {
        IEnumerable<ICMViewModel> GetAllAsync();
        //bool HasDuplicate(ResponseViewModel request);
        void Add(ResponseViewModel entity);
        void Add(ICMViewModel entity);
    }
}
