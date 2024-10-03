using AutoMapper;
using Resturant.Core;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Data
{
    public class BaseResturantDbGuid<TEntity> : BaseRepositoryGuid<TEntity>
   where TEntity : BaseEntityGuid, new()
    {
        #region Private Members

        protected ResturantDbContext ResturantDbContext { get; }
        protected IStringLocalizer<SharedResource> Localizer { get; }

        
        #endregion

        #region Contsructors

        protected BaseResturantDbGuid(ResturantDbContext context, IMapper mapper, IStringLocalizer<SharedResource> localizer) : base(context, mapper, localizer)
        {
            ResturantDbContext = context;
            Localizer = localizer;
        }

        #endregion
    }

    public class BaseResturantDbLong<TEntity> : BaseRepositoryLong<TEntity>
    where TEntity : BaseEntityLong, new()
    {
        #region Private Members

        protected ResturantDbContext ResturantDbContext { get; }
        protected IStringLocalizer<SharedResource> Localizer { get; }


        #endregion

        #region Contsructors

        protected BaseResturantDbLong(ResturantDbContext context, IMapper mapper, IStringLocalizer<SharedResource> localizer) : base(context, mapper, localizer)
        {
            ResturantDbContext = context;
            Localizer = localizer;
        }

        #endregion
    }
}
