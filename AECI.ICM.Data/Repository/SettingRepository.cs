using AECI.ICM.Data.Context;
using AECI.ICM.Data.Entities;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using AECI.ICM.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AECI.ICM.Data.Repository
{
    public class SettingRepository : ISettingRepository
    {
        #region Readonly Fields

        private ICMDbContext _ctx;

        #endregion

        #region Constructor

        public SettingRepository(ICMDbContext ctx)
        {
            _ctx = ctx;
        }

        #endregion

        #region Methods

        public SettingEntity GetAllAsync()
        {
            try
            {
                var results =  _ctx.Settings.Include(p => p.Emails).FirstOrDefault();

                if (results == null)
                    return new SettingEntity();                
             
                return Map(results);
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                throw;
            }
        }

        public bool GetGMStatus(string site, string email)
        {
            var settings = GetAllAsync().Emails.Where(p=>p.Site == site && p.GMEmail == email).ToList();

            return settings.Count > 0;
        }

        public string GetRegion(string site)
        {
            var setting = GetAllAsync().Emails.Where(p => p.Site == site).FirstOrDefault();

            return setting.Region;
        }

        public void SaveSetting(SettingEntity param)
        {
            try
            {
                var mappedEntity = ReverseMap(param);

                if (mappedEntity.Id == 0)
                {
                    _ctx.Settings.Add(mappedEntity);
                }
                if(mappedEntity.Emails.Any(p=>p.Id == 0))
                {
                    mappedEntity.Emails.ForEach(x =>
                    {
                        if(x.Id == 0)
                        {
                           if(x.SettingId ==0) x.SettingId = param.Id.GetId();
                            _ctx.SettingEmails.Add(x);
                        }
                    });
                }
                else
                {
                    var currEntity = _ctx.Settings.Include(
                        p => p.Emails).Where(
                        p => p.Id == mappedEntity.Id).FirstOrDefault();

                    if (currEntity == null)
                        throw new ArgumentNullException(nameof(currEntity), 
                            "Entity cannot be null - Update EF Core");

                    foreach (var entity in mappedEntity.Emails)
                    {
                        currEntity.Emails.FirstOrDefault(
                            p => p.Id == entity.Id &&
                            p.BranchManagerEmail == entity.BranchManagerEmail)
                            .BranchManagerEmail = entity.BranchManagerEmail;

                        currEntity.Emails.FirstOrDefault(
                            p => p.Id == entity.Id &&
                            p.BranchManagerEmail == entity.BranchManagerEmail)
                            .BranchManagerName = entity.BranchManagerName;

                        currEntity.Emails.FirstOrDefault(
                            p => p.Id == entity.Id &&
                            p.BranchManagerEmail == entity.BranchManagerEmail)
                            .Id = entity.Id;

                        currEntity.Emails.FirstOrDefault(
                            p => p.Id == entity.Id &&
                            p.BranchManagerEmail == entity.BranchManagerEmail)
                            .RegionalAccountantEmail = entity.RegionalAccountantEmail;

                        currEntity.Emails.FirstOrDefault(
                            p => p.Id == entity.Id &&
                            p.BranchManagerEmail == entity.BranchManagerEmail)
                            .RegionalAccountantName = entity.RegionalAccountantName;

                        currEntity.Emails.FirstOrDefault(
                            p => p.Id == entity.Id &&
                            p.BranchManagerEmail == entity.BranchManagerEmail)
                            .SettingId = entity.SettingId;

                        currEntity.Emails.FirstOrDefault(
                            p => p.Id == entity.Id &&
                            p.BranchManagerEmail == entity.BranchManagerEmail)
                            .Site = entity.Site;
                    }

                    currEntity.EnableWarning = mappedEntity.EnableWarning;
                    currEntity.SignatureLocation = mappedEntity.SignatureLocation;
                    currEntity.WarningCuttOffDate = mappedEntity.WarningCuttOffDate;
                    currEntity.WarningEmail = mappedEntity.WarningEmail;

                    _ctx.Settings.Update(currEntity);
                }
                _ctx.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private Methods

        private SettingEntity Map(Setting settings)
        {
            var mapped = new SettingEntity();

            mapped.EnableWarning = settings.EnableWarning;
            mapped.Id = new SettingId(settings.Id);
            mapped.SignatureLocation = settings.SignatureLocation;
            mapped.WarningCuttOffDate = settings.WarningCuttOffDate;
            mapped.WarningEmail = settings.WarningEmail;
            mapped.OnlineApiUrl = settings.OnlineApiUrl;

            settings.Emails.ForEach(p =>
            {
                mapped.Emails.Add(new SettingEmailEntity
                {
                    BranchManagerEmail = p.BranchManagerEmail,
                    BranchManagerName = p.BranchManagerName,
                    Id = p.Id,
                    SettingId = settings.Id,
                    RegionalAccountantEmail = p.RegionalAccountantEmail,
                    RegionalAccountantName = p.RegionalAccountantName,
                    Site = p.Site,
                    Active = p.Active,
                    GMEmail = p.GMEmail,
                    GMName = p.GMName,
                    Region = p.Region           
                });
            });

            return mapped;
        }

        private Setting ReverseMap(SettingEntity settings)
        {
            var reverse = new Setting();

            reverse.EnableWarning = settings.EnableWarning;
            reverse.Id = settings.Id.GetId();
            reverse.SignatureLocation = settings.SignatureLocation;
            reverse.WarningCuttOffDate = settings.WarningCuttOffDate;
            reverse.WarningEmail = settings.WarningEmail;
            reverse.OnlineApiUrl = settings.OnlineApiUrl;

            settings.Emails.ForEach(p =>
            {
                reverse.Emails.Add(new SettingEmail
                {
                    BranchManagerEmail = p.BranchManagerEmail,
                    BranchManagerName = p.BranchManagerName,
                    Id = p.Id,
                    SettingId = settings.Id.GetId(),
                    Site = p.Site,
                    RegionalAccountantEmail = p.RegionalAccountantEmail,
                    RegionalAccountantName = p.RegionalAccountantName
                });
            });

            return reverse;
        }

        #endregion
    }
}
