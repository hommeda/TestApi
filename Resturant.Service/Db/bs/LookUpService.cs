using Resturant.Core;
using Resturant.Core.Utilities;
using Resturant.Services;

namespace Resturant.Service
{
    public class LookUpService : BaseService, ILookUpService
    {
        #region Private Members

        private readonly IUnitOfWorkDb _iUnitOfWorkDb;
        //private readonly string[] _includes = { "Category", "Brand" };

        #endregion

        #region Public Members

        #endregion

        #region Contsructors

        public LookUpService(IUnitOfWorkDb iUnitOfWorkDb)
        {
            _iUnitOfWorkDb = iUnitOfWorkDb;
        }

        #endregion

        #region Private Functions

        #endregion

        #region Public Functions

        public async Task<ApiObjectData> Get(long id)
        {
            return await _iUnitOfWorkDb.LookUp.Get<LookUpDto>(id);
        }

        public async Task<ApiObjectData> Get()
        {
            return await _iUnitOfWorkDb.LookUp.Get<LookUpDto>();
        }

        public async Task<ApiObjectData> Save(BaseEntityLongVm vm)
        {
            return await _iUnitOfWorkDb.LookUp.Save(vm);
        }

        public async Task<ApiObjectData> Save(LookUpVm vm)
        {
            var data = new ApiObjectData();
            try
            {
                var saved = await _iUnitOfWorkDb.LookUp.Save(vm);
                data =await Get();
                if (saved.Message.Type == "Success") data.Message.MsgSuccess();
                else data.Message = saved.Message;
            }
            catch (Exception ex)
            {
                data.ReturnData = null;
                data.Message.MsgError(ex.ExceptionMessage());
            }
            return data;

        }

        public async Task<ApiObjectData> Remove(long id)
        {
            return await _iUnitOfWorkDb.LookUp.Remove(id);
        }

        public async Task<ApiObjectData> Delete(long id)
        {
            return await _iUnitOfWorkDb.LookUp.Delete(id);
        }

        #endregion

        #region Dispose

        private bool _disposedValue;

        protected override void Dispose(bool disposing)
        {
            if (!_disposedValue && disposing) _iUnitOfWorkDb.Dispose();
            _disposedValue = true;
        }

        #endregion
    }
}
