using AutoMapper;
using Microsoft.Extensions.Localization;
using Resturant.Core;

namespace Resturant.Data
{
    public class LookUpRepository: BaseResturantDbLong<LookUp>, ILookUpRepository
    {
        #region Private Members

        #endregion

        #region Public Members
        #endregion

        #region Contsructors
        public LookUpRepository(ResturantDbContext context, IMapper mapper, IStringLocalizer<SharedResource> localizer) : base(context, mapper, localizer)
        {
        }

        #endregion

        #region Private Functions

        #endregion

        #region Public Functions
        public async Task<ApiObjectData> Get(long id)
        {
            return await Get<LookUpDto>(id);
        }

        public async Task<ApiObjectData> Get()
        {
            return await Get<LookUpDto>();
        }
        public async Task<ApiObjectData> Save(BaseEntityLongVm vm)
        {
            return await Save<LookUpVm>(vm);
        }

        #endregion

        #region Dispose

        private bool _disposedValue;
        protected override void Dispose(bool disposing)
        {
            if (!_disposedValue && disposing) ResturantDbContext.Dispose();
            _disposedValue = true;
        }

        #endregion
    }
}
