using AECI.ICM.Application.Interfaces;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using System;

namespace AECI.ICM.Application.Services
{
    public class SettingsService : ISettingsService
    {
        #region Readonly Fields

        private readonly ISettingRepository _settingsRepo;
      
        #endregion

        #region Constructor

        public SettingsService(ISettingRepository settingsRepo)
        {
            _settingsRepo = settingsRepo;
        }

        #endregion

        #region Methods

        public SettingEntity GetAllAsync()
        {
            try
            {
                var results = _settingsRepo.GetAllAsync();

                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GetGMStatus(string branch, string email)
        {
            return _settingsRepo.GetGMStatus(branch, email);
        }

        public string GetRegion(string branch)
        {
            return _settingsRepo.GetRegion(branch);
        }

        public void SaveSettingAsync(SettingEntity entity)
        {
            try
            {
                _settingsRepo.SaveSetting(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
