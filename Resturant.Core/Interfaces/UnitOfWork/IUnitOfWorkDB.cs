namespace Resturant.Core
{
    public interface IUnitOfWorkDb : IDisposable
    {

        #region Bs 
        ILookUpRepository LookUp { get; }
        #endregion

        Task<int> CommitAsync();
    }
}
