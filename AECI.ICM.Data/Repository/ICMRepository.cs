using AECI.ICM.Data.AutoMapper;
using AECI.ICM.Data.Context;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using AECI.ICM.Domain.ValueObjects;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AECI.ICM.Data.Repository
{
    public class ICMRepository : IICMRepository
    {
        #region Readonly Fields

        private readonly ICMDbContext _ctx;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public ICMRepository(ICMDbContext ctx)
        {
            _ctx = ctx;          
           _mapper = new EntitiesMap().Create();
        }

        #endregion

        #region Methods

        public IEnumerable<ICMEntity> GetAllAsync()
        {
            var result = _ctx.ICM.Include(p=>p.SectionDetail).ToList();

            return Map(result);
        }

        public IEnumerable<ICMEntity>Map(List<Entities.ICM>source)
        {
            var destination = new List<ICMEntity>();

            foreach (Entities.ICM item in source)
            {
                var entity = new ICMEntity(new ICMId(item.Id));
                entity.ControlStatement = item.ControlStatement;
                entity.FinanceFunctionCheck = item.FinanceFunctionCheck;
                entity.BranchManager = item.BranchManager;
                entity.RegionalAccountant = item.RegionalAccountant;
                entity.Section = item.SectionDetail.Section;
                entity.SectionName = item.SectionDetail.SectionName;
                entity.Comments = item.Comments;

                destination.Add(entity);
            }

            return destination;
        }

        #endregion
    }
}
