using AECI.ICM.Data.AutoMapper;
using AECI.ICM.Data.Context;
using AECI.ICM.Data.Entities;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using AECI.ICM.Domain.ValueObjects;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
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

        public void Add(ResultEntity entity)
        {
            try
            {
                var result = Map(entity);
                _ctx.Results.Add(result);

                _ctx.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Add(ICMEntity entity)
        {
            try
            {
                if (entity.Id.ToString() == null)
                {
                    _ctx.ICM.Add(Map(entity));
                }
                else
                {
                    var currICM = _ctx.ICM.Include(p=>p.SectionDetail).FirstOrDefault(p => p.Id == entity.Id.GetId());

                    if (currICM == null)
                        throw new ArgumentNullException();

                    currICM.BranchManager = entity.BranchManager;
                    currICM.Comments = entity.Comments;
                    currICM.ControlStatement = entity.ControlStatement;
                    currICM.FinanceFunctionCheck = entity.FinanceFunctionCheck;
                    currICM.RegionalAccountant = entity.RegionalAccountant;


                    currICM.SectionDetail.Section = entity.Section;
                    currICM.SectionDetail.SectionName = entity.SectionName;

                    _ctx.ICM.Update(currICM);
                  
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

        private IEnumerable<ICMEntity> Map(List<Entities.ICM> source)
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

        private Result Map(ResultEntity source)
        {
            var mapped = new Result();

            mapped.BranchManagerName = source.BMName;
            mapped.Branch = source.Branch;
            mapped.Date = source.Date;
            mapped.DateSigned = source.DateSigned;
            mapped.FinanceName = source.FinName;
            mapped.Month = source.Month;
            //mapped.RegionalAccountantName = source.RegionalACC;

            return mapped;
        }

        private Entities.ICM Map(ICMEntity source)
        {
            var mapped = new Entities.ICM();

            mapped.BranchManager = source.BranchManager;
            mapped.Comments = source.Comments;
            mapped.ControlStatement = source.ControlStatement;
            mapped.FinanceFunctionCheck = source.FinanceFunctionCheck;
            mapped.RegionalAccountant = source.RegionalAccountant ;           
            mapped.BranchManager = source.BranchManager;
            mapped.SectionDetail = new Entities.SectionDetail()
            {
                Section = source.Section,
                SectionName = source.SectionName
            };

            return mapped;
        }

        #endregion
    }
}
