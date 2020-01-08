using AECI.ICM.Application.Interfaces;
using System.Collections.Generic;

namespace AECI.ICM.Application.Models
{
    public abstract class Region
    {
        public abstract List<string> GetAbbrevList(List<IBranch> branches);     
    }
}
