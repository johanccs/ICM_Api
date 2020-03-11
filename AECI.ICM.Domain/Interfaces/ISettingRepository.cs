using AECI.ICM.Domain.Entities;
using System.Threading.Tasks;

namespace AECI.ICM.Domain.Interfaces
{
    public interface ISettingRepository
    {
        SettingEntity GetAllAsync();
        
        void SaveSetting(SettingEntity param);

        string GetRegion(string branch);

        bool GetGMStatus(string branch, string email);
    }
}
