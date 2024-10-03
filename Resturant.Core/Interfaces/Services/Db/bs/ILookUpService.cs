namespace Resturant.Core
{
    public interface ILookUpService : IBaseLong
    {
        Task<ApiObjectData> Save(LookUpVm vm);
    }
}
