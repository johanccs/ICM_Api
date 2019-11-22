using AECI.ICM.Domain.Entities;
using System.Threading.Tasks;

namespace AECI.ICM.Application.Interfaces
{
    public interface ISettingsService
    {
        void SaveSettingAsync(SettingEntity entity);
        SettingEntity GetAllAsync();
    }
}
