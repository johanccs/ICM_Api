using AECI.ICM.Application.Interfaces;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using System.Collections.Generic;

namespace AECI.ICM.Application.Services
{
    public class ResultService : IResultService
    {
        #region Readonly Fields

        private readonly IResultRepository _resultRepository;

        #endregion

        #region Constructor

        public ResultService(IResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }

        #endregion

        #region Methods

        public IEnumerable<ResultEntity> GetAllAsync()
        {
            var results =_resultRepository.GetAllAsync();

            return results;
        }

        #endregion
    }
}
