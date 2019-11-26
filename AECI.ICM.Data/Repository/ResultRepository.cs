using AECI.ICM.Data.Context;
using AECI.ICM.Data.Entities;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<ResultEntity> GetAllAsync()
        {
            var results = Map(_context.Results.ToList());

            return results;
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
                    Month = p.Month
                });
            });

            return mapped;
        }

        #endregion
    }
}
