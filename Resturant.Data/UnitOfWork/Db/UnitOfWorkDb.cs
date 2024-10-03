using AutoMapper;
using Resturant.Core;
using Microsoft.Extensions.Localization;

namespace Resturant.Data
{
    public class UnitOfWorkDb : BaseUnitOfWork<ResturantDbContext>, IUnitOfWorkDb
    {

        #region Contsructors

        public UnitOfWorkDb(ResturantDbContext context, ResturantDbDapperContext dbDapperContext, IMapper mapper, IStringLocalizer<SharedResource> localizer) : base(context, mapper, localizer)
        {
        }

        #endregion

        #region Private Members

        #region Bs

        private LookUpRepository _lookUpRepository;
        #endregion

        #endregion

        #region Public Members

        #region BS

        public ILookUpRepository LookUp => _lookUpRepository ??=new LookUpRepository(Context, Mapper, Localizer);

        #endregion


        #endregion
    }
}
