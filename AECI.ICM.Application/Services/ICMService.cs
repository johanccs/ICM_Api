using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.Interfaces;
using System.Collections.Generic;

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

        public IEnumerable<ICMEntity> GetAllAsync()
        {
            return _icmRepository.GetAllAsync();
        }
    }
}
