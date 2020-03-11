using AECI.ICM.Application.Interfaces;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<ResultEntity>> GetAllAsync()
        {
            var results = await _resultRepository.GetAllAsync();

            return results;
        }

        public async Task<IEnumerable<ResultEntity>> GetAllAsync(string region)
        {
            var results = await _resultRepository.GetAllAsync(region);

            return results;
        }

        public Task<bool> Authorise(ResultEntity entity)
        {
            return _resultRepository.Authorise(entity);
        }

        #endregion
    }
}
