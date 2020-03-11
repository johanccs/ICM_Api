using AECI.ICM.Data.Context;
using AECI.ICM.Data.Entities;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AECI.ICM.Data.Repository
{
    public class ResultRepository : IResultRepository
    {
        #region Readonly Fields

        private readonly ICMDbContext _context;

        #endregion

        #region Constructor

        public ResultRepository(ICMDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<ResultEntity>> GetAllAsync()
        {
            var results =  await _context.Results.ToListAsync();

            return Map(results);
        }

        public async Task<IEnumerable<ResultEntity>> GetAllAsync(string region)
        {
            var results = await GetAllAsync();

            return results.Where(p=>p.Region.ToLower() == region.ToLower() && 
                                 p.GMAuthorisedStatus == 0).ToList();
        }

        public async Task<bool> Authorise(ResultEntity entity)
        {
            try
            {
                if (entity.Id == 0)
                    throw new Exception("Result seems to be new");

                if (entity.GMAuthorisedStatus > 0)
                    throw new Exception("Result is already authorised");

                var existingResult = _context.Results
                    .FirstOrDefault(p => p.Id == entity.Id && 
                                    p.Region == entity.Region && 
                                    p.Branch == p.Branch);

                if (existingResult == null)
                    throw new Exception("Result not found in database");

                existingResult.Id = entity.Id;
                //existingResult.BMAuthorisedStatus = entity.BMAuthorisedStatus;
                //existingResult.Branch = entity.Branch;
                //existingResult.BranchManagerName = entity.BMName;
                //existingResult.Date = entity.Date;
                //existingResult.DateSigned = entity.DateSigned;
                //existingResult.FinanceName = entity.FinName;
                existingResult.GMAuthorisedStatus = 1;
                //existingResult.Month = entity.Month;
                //existingResult.Region = entity.Region;
                 _context.Update(existingResult);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private Methods

        private List<ResultEntity> Map(List<Result>unmapped)
        {
            var mapped = new List<ResultEntity>();

            unmapped.ForEach(p =>
            {
                mapped.Add(new ResultEntity
                {
                    BMName = p.BranchManagerName,
                    Branch = p.Branch,
                    Date = p.Date,
                    DateSigned = p.DateSigned,
                    FinName = p.FinanceName,
                    Id = p.Id,
                    Month = p.Month,
                    BMAuthorisedStatus = p.BMAuthorisedStatus,
                    GMAuthorisedStatus = p.GMAuthorisedStatus,
                    Region = p.Region
                });
            });

            return mapped;
        }

        #endregion
    }
}
