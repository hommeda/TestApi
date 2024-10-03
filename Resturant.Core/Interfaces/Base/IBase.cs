using System;
using System.Threading.Tasks;

namespace Resturant.Core
{
    public interface IBase : IDisposable
    {
        Task<ApiObjectData> Get();
    }

    public interface IBaseLong : IBase
    {
        Task<ApiObjectData> Get(long id);
        Task<ApiObjectData> Save(BaseEntityLongVm vm);
        Task<ApiObjectData> Remove(long id);
        Task<ApiObjectData> Delete(long id);
    }

    public interface IBaseGuid : IBase
    {
        Task<ApiObjectData> Get(Guid id);
        Task<ApiObjectData> Save(BaseEntityGuidVm vm);
        Task<ApiObjectData> Remove(Guid id);
        Task<ApiObjectData> Delete(Guid id);
    }
}
