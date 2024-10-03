using System;

namespace Resturant.Services
{
    public class BaseService : IDisposable
    {
        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
