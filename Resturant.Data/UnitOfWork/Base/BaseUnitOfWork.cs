using AutoMapper;
using Resturant.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace Resturant.Data
{
    public class BaseUnitOfWork<TDbContext>
         where TDbContext : DbContext
    {
        #region Private Members

        protected readonly TDbContext Context;
        protected readonly IMapper Mapper;
        protected readonly IStringLocalizer<SharedResource> Localizer;

        #endregion

        #region Contsructors

        protected BaseUnitOfWork(TDbContext context, IMapper mapper, IStringLocalizer<SharedResource> localizer)
        {
            Context = context;
            Mapper = mapper;
            Localizer = localizer;
        }

        #endregion

        #region Public Functions

        public async Task<int> CommitAsync()
        {
            return await Context.SaveChangesAsync();
        }

        #endregion

        #region Dispose

        private bool _disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue && disposing) Context.Dispose();
            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
