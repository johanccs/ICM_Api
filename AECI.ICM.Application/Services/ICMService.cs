using AECI.ICM.Data.DataExceptions;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using AECI.ICM.Shared.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AECI.ICM.Application.Services
{
    public class ICMService : IICMService
    {
        #region Readonly Fields

        private readonly IICMRepository _icmRepository;

        #endregion

        #region Constructor

        public ICMService(IICMRepository icmRepository)
        {
            _icmRepository = icmRepository;
        }

        #endregion

        #region Public Methods

        public IEnumerable<ICMViewModel> GetAllAsync()
        {
            var result = _icmRepository.GetAllAsync().ToList();

            var mapped = Map(result);

            return mapped;
        }

        public void Add(ResponseViewModel entity)
        {
            try
            {
                if (entity.BMAuthorisedStatus == 0)
                {
                    entity.BMAuthorisedStatus = 1;
                    entity.GMAuthorisedStatus = 0;
                }
                _icmRepository.Add(Map(entity));
            }            
            catch (System.Exception)
            {
                throw;
            }
        }

        public void Add(ICMViewModel entity)
        {
            var result = Map(entity);
            _icmRepository.Add(result);
        }

        #endregion

        #region Private Methods

        private ResultEntity Map(ResponseViewModel unmapped)
        {
            var mapped = new ResultEntity();
            mapped.BMName = unmapped.BMName;
            mapped.Branch = unmapped.Branch;
            mapped.Date = unmapped.Date;
            mapped.DateSigned = unmapped.DateSigned;
            mapped.FinName = unmapped.FinName;
            mapped.Month = unmapped.Month;
            mapped.BMAuthorisedStatus = unmapped.BMAuthorisedStatus;
            mapped.GMAuthorisedStatus = unmapped.GMAuthorisedStatus;
            mapped.Region = unmapped.Region;
            mapped.GMName = unmapped.GMName;
            //mapped.RegionalACC = unmapped.RegionalAcc;

            return mapped;
        }
        
        private List<ICMViewModel>Map(List<ICMEntity>unmapped)
        {
            var mapped = new List<ICMViewModel>();

            unmapped.ForEach(p =>
            {
                mapped.Add(new ICMViewModel
                {
                    BranchManager = p.BranchManager,
                    Comments = p.Comments,
                    ControlStatement = p.ControlStatement,
                    FinanceFunctionCheck  = p.FinanceFunctionCheck,
                    RegionalAccountant = p.RegionalAccountant,
                    Section = p.Section,
                    SectionName = p.SectionName ,
                    Id = p.Id.GetId()
                });
            });

            return mapped;
        }

        private ICMEntity Map(ICMViewModel unmapped)
        {
            var mapped = new ICMEntity();

            mapped.BranchManager = unmapped.BranchManager;
            mapped.Comments = unmapped.Comments;
            mapped.ControlStatement = unmapped.ControlStatement;
            mapped.Id = new Domain.ValueObjects.ICMId(unmapped.Id);
            mapped.RegionalAccountant = unmapped.RegionalAccountant;
            mapped.Section = unmapped.Section;
            mapped.SectionName = unmapped.SectionName;


            return mapped;
        }

        #endregion
    }
}
