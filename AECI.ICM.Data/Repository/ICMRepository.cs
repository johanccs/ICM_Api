using AECI.ICM.Data.Context;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AECI.ICM.Data.Repository
{
    public class ICMRepository : IICMRepository
    {
        #region Readonly Fields

        private readonly ICMDbContext _ctx;

        #endregion

        #region Constructor

        public ICMRepository(ICMDbContext ctx)
        {
            _ctx = ctx;
        }

        #endregion

        #region Methods

        public IEnumerable<ICMEntity> GetAllAsync()
        {
            var result = _ctx.ICM.Include(p=>p.SectionDetail).ToList();

            return new List<ICMEntity>();
        }

        #endregion
    }
}
