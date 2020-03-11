using AECI.ICM.Domain.Entities;

namespace AECI.ICM.Application.Interfaces
{
    public interface ISettingsService
    {
        void SaveSettingAsync(SettingEntity entity);
        
        SettingEntity GetAllAsync();

        string GetRegion(string branch);

        bool GetGMStatus(string branch, string email);
        
    }
}
