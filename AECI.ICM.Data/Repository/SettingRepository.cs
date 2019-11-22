using AECI.ICM.Data.Context;
using AECI.ICM.Data.Entities;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using AECI.ICM.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                //var results = _ctx.Settings.FirstOrDefault();

                return Map(results);
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                throw;
            }
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

                    mappedEntity.Emails.ForEach(p =>
                    {
                        currEntity.Emails.ForEach(y =>
                        {
                            y.BranchManagerEmail = p.BranchManagerEmail;
                            y.BranchManagerName = p.BranchManagerName;
                            y.RegionalAccountantEmail = p.RegionalAccountantEmail;
                            y.RegionalAccountantName = p.RegionalAccountantName;
                            y.Site = p.Site;
                        });
                    });

                    currEntity.EnableWarning = mappedEntity.EnableWarning;
                    currEntity.SignatureLocation = mappedEntity.SignatureLocation;
                    currEntity.WarningCuttOffDate = mappedEntity.WarningCuttOffDate;
                    currEntity.WarningEmail = mappedEntity.WarningEmail;

                    _ctx.Settings.Update(currEntity);
                }
                _ctx.SaveChanges();
            }
            catch (Exception ex)
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
                    Site = p.Site
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
