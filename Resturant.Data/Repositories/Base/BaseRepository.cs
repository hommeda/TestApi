using AutoMapper;
using Resturant.Core;
using Resturant.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Resturant.Core.Utilities.Enums;

namespace Resturant.Data
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
              where TEntity : BaseEntity, new()
    {
        #region Private Members
        #endregion

        #region Protected Members

        protected DbSet<TEntity> EntitySet => _context.Set<TEntity>();
        protected readonly DbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IStringLocalizer<SharedResource> _localizer;

        #endregion

        #region Public Members
        #endregion

        #region Contsructors

        protected BaseRepository(DbContext context, IMapper mapper, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        #endregion

        #region Protected Functions

        protected void UpdateColumnsStatus(TEntity entity, IEnumerable<string> columns, bool status)
        {
            foreach (var column in columns)
            {
                _context.Entry(entity).Property(column).IsModified = status;
            }
        }

        protected Expression<Func<T, bool>> UpdateParameter<T>(Expression<Func<T, bool>> expr, ParameterExpression newParameter)
        {
            var visitor = new ParameterUpdateVisitor(expr.Parameters[0], newParameter);
            var body = visitor.Visit(expr.Body);

            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }

        #endregion

        #region Public Functions

        #region Base Functions

        public async void Add(TEntity entity)
        {
            await EntitySet.AddAsync(entity);
        }

        public void AddRange(IEnumerable<TEntity> entity)
        {
            EntitySet.AddRange(entity);
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        //public async Task<bool> Save(TEntity entity)
        //{
        //    if (entity.Id == 0)
        //    {
        //        Add(entity);
        //    }
        //    return await Save();
        //}

        //public async void Delete(long id)
        //{
        //    EntitySet.Remove(await Find(id));
        //}

        public void Delete(TEntity entity)
        {
            EntitySet.Remove(entity);
        }

        public T Map<T>(TEntity entity) where T : class
        {
            return _mapper.Map<T>(entity);
        }

        public async Task<IEnumerable<TEntity>> Find()
        {
            return await EntitySet.Where(o => o.IsDeleted == false).ToListAsync();
        }

        public async Task<TEntity> First(Expression<Func<TEntity, bool>> predicate)
        {
            var data = await EntitySet.FirstOrDefaultAsync(predicate);
            return data;
        }

        public async Task<TEntity> First(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet.Where(o => o.IsDeleted == false), (current, child) => current.Include(child));
            var data = await query.FirstOrDefaultAsync(predicate); ;
            return data;
        }

        public async Task<TEntity> Last(Expression<Func<TEntity, bool>> predicate)
        {
            //var data = await EntitySet.LastOrDefaultAsync(predicate);
            //return data;
            var data = await EntitySet.OrderBy(x => x.CreatedDate).LastOrDefaultAsync(predicate);
            return data;
        }

        public async Task<IEnumerable<TEntity>> Filter(params string[] includes)
        {
            var query = includes.Aggregate(EntitySet.Where(o => o.IsDeleted == false), (current, child) => current.Include(child));
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            Expression<Func<TEntity, bool>> where = q => q.IsDeleted == false;
            predicate = UpdateParameter(predicate, where.Parameters[0]);
            var data = await EntitySet.Where(predicate).ToListAsync();
            return data;
        }
        public async Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderby, OrderType orderType)
        {
            Expression<Func<TEntity, bool>> where = q => q.IsDeleted == false;
            predicate = UpdateParameter(predicate, where.Parameters[0]);
            var data = orderType == OrderType.DESC ? await EntitySet.Where(predicate).OrderByDescending(orderby).ToListAsync() : await EntitySet.Where(predicate).OrderBy(orderby).ToListAsync();
            return data;
        }
        public async Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderby)
        {
            Expression<Func<TEntity, bool>> where = q => q.IsDeleted == false;
            predicate = UpdateParameter(predicate, where.Parameters[0]);
            var data = await EntitySet.Where(predicate).OrderBy(orderby).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<TEntity>> OrderBy(Expression<Func<TEntity, object>> orderby)
        {
            var data = await EntitySet.Where(o => o.IsDeleted == false).OrderBy(orderby).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet.Where(o => o.IsDeleted == false), (current, child) => current.Include(child));
            Expression<Func<TEntity, bool>> where = q => q.IsDeleted == false;
            predicate = UpdateParameter(predicate, where.Parameters[0]);
            var data = await query.Where(predicate).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderby, OrderType orderType, params string[] includes)
        {
            var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet.Where(o => o.IsDeleted == false), (current, child) => current.Include(child));
            Expression<Func<TEntity, bool>> where = q => q.IsDeleted == false;
            predicate = UpdateParameter(predicate, where.Parameters[0]);
            var data = orderType == OrderType.DESC ? await query.Where(predicate).OrderByDescending(orderby).ToListAsync() : await query.Where(predicate).OrderBy(orderby).ToListAsync();
            return data;
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 20, params string[] includes)
        {
            var query = includes.Aggregate(EntitySet.Where(o => o.IsDeleted == false), (current, child) => current.Include(child));
            var skipCount = index * size;
            var resetSet = filter != null ? query.Where(filter).AsQueryable() : query.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet;
        }

        public async Task<IEnumerable<TEntity>> Query(Expression<Func<TEntity, bool>> filter, string children)
        {
            return await EntitySet.Include(children).Where(filter).ToListAsync();
        }

        //public async void MarkAsDelete(long id)
        //{
        //    (await Find(id)).IsDeleted = true;
        //}

        //public async Task<TEntity> Find(long id)
        //{
        //    return await EntitySet.Where(o => o.Id == id && o.IsDeleted == false).FirstOrDefaultAsync();
        //}

        //public async Task<TEntity> Filter(long id, params string[] includes)
        //{
        //    var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet, (current, child) => current.Include(child));
        //    var data = await query.Where(o => o.Id == id && o.IsDeleted == false).FirstOrDefaultAsync();
        //    return data;
        //}

        #endregion

        #region Extra Functions

        //public async Task<ApiObjectData> Get<T>(long id) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = await Find(id);
        //        data.ReturnData = _mapper.Map<T>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //protected async Task<ApiObjectData> Get<T>(long id, params string[] includes) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = await Filter(id, includes);
        //        data.ReturnData = _mapper.Map<T>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        public async Task<ApiObjectData> GetFirst<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = (await Filter(predicate)).FirstOrDefault();
                data.ReturnData = _mapper.Map<T>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> GetLast<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = (await Filter(predicate)).LastOrDefault();
                data.ReturnData = _mapper.Map<T>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Get<T>() where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = await Find();
                data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Get<T>(params string[] includes) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = await Filter(includes);
                data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = await Filter(predicate);
                data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderby, OrderType orderType) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = await Filter(predicate, orderby, orderType);
                data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate, params string[] includes) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = await Filter(predicate, includes);
                data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderby, OrderType orderType, params string[] includes) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = await Filter(predicate, orderby, orderType, includes);
                data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Get<T>(Expression<Func<TEntity, object>> orderby) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = await OrderBy(orderby);
                data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        //public async Task<ApiObjectData> Save<T>(BaseEntityVm entityVm) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var entity = new TEntity();
        //        if (entityVm.Id != 0)
        //        {
        //            entity = await Find(entityVm.Id);
        //        }
        //        entity = _mapper.Map(entityVm, entity);
        //        if (entityVm.Id != 0)
        //        {
        //            entity.ModifiedDate = DateTime.Now;
        //            entity.ModifiedBy = entityVm.UserId;
        //            UpdateColumnsStatus(entity, new[] { "CreatedBy", "CreatedDate", "IsDeleted", "InActive", }, false);
        //        }
        //        else
        //        {
        //            entity.CreatedDate = DateTime.Now;
        //            entity.CreatedBy = entityVm.UserId;
        //        }
        //        if (await Save(entity))
        //        {
        //            data.ReturnData = _mapper.Map<T>(entity);
        //            data.Message.MsgSuccess();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        data.ReturnData = entityVm;
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> Save<T>(BaseEntityVm entityVm, params string[] includes) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var entity = new TEntity();
        //        if (entityVm.Id != 0)
        //        {
        //            entity = await Find(entityVm.Id);
        //        }
        //        entity = _mapper.Map(entityVm, entity);
        //        if (entityVm.Id != 0)
        //        {
        //            entity.ModifiedDate = DateTime.Now;
        //            entity.ModifiedBy = entityVm.UserId;
        //            UpdateColumnsStatus(entity, new[] { "CreatedBy", "CreatedDate", "IsDeleted", "InActive" }, false);
        //        }
        //        else
        //        {
        //            entity.CreatedDate = DateTime.Now;
        //            entity.CreatedBy = entityVm.UserId;
        //        }
        //        if (await Save(entity))
        //        {
        //            entity = await Filter(entity.Id, includes);
        //            data.ReturnData = _mapper.Map<T>(entity);
        //            data.Message.MsgSuccess();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        data.ReturnData = entityVm;
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> Remove(long id)
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var entity = await Find(id);
        //        if (entity != null)
        //        {
        //            entity.IsDeleted = true;
        //            entity.ModifiedDate = DateTime.Now;
        //            UpdateColumnsStatus(entity, new[] { "CreatedBy", "CreatedDate", "InActive" }, false);
        //            await Save();
        //            data.Message.MsgSuccess(_localizer["DeleteMsgSuccess"]);
        //        }
        //        else
        //            data.Message.MsgError(_localizer["UnexpectedError"]);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> Delete(long id)
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var entity = await Find(id);
        //        if (entity != null)
        //        {
        //            Delete(entity);
        //            await Save();
        //            data.Message.MsgSuccess(_localizer["DeleteMsgSuccess"]);
        //        }
        //        else
        //            data.Message.MsgError(_localizer["UnexpectedError"]);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        #endregion

        #endregion

        #region Dispose

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue && disposing) _context.Dispose();
            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual Task<bool> Save(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Refresh(TEntity entity)
        {
            _context.Entry(entity).Reload();
        }

        #endregion
    }

    public class BaseRepositoryLong<TEntity> : BaseRepository<TEntity>, IBaseRepositoryLong<TEntity>
    where TEntity : BaseEntityLong, new()
    {
        #region Private Members

        #endregion

        #region Protected Members

        #endregion

        #region Public Members
        #endregion

        #region Contsructors

        protected BaseRepositoryLong(DbContext context, IMapper mapper, IStringLocalizer<SharedResource> localizer) : base(context, mapper, localizer)
        {
        }

        #endregion

        #region Private Functions

        #endregion

        #region Public Functions

        #region Base Functions

        //public async void Add(TEntity entity)
        //{
        //    await EntitySet.AddAsync(entity);
        //}

        //public void AddRange(IEnumerable<TEntity> entity)
        //{
        //    EntitySet.AddRange(entity);
        //}

        //public async Task<bool> Save()
        //{
        //    return await _context.SaveChangesAsync() > 0;
        //}

        public override async Task<bool> Save(TEntity entity)
        {
            if (entity.Id == 0)
            {
                Add(entity);
            }
            return await Save();
        }

        //public void Delete(TEntity entity)
        //{
        //    EntitySet.Remove(entity);
        //}

        public async void MarkAsDelete(long id)
        {
            (await Find(id)).IsDeleted = true;
        }

        //public T Map<T>(TEntity entity) where T : class
        //{
        //    return _mapper.Map<T>(entity);
        //}

        public async Task<TEntity> Find(long id)
        {
            return await EntitySet.Where(o => o.Id == id && o.IsDeleted == false).FirstOrDefaultAsync();
        }

        //public async Task<IEnumerable<TEntity>> Find()
        //{
        //    return await EntitySet.Where(o => o.IsDeleted == false).ToListAsync();
        //}

        public async Task<TEntity> Filter(long id, params string[] includes)
        {
            var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet, (current, child) => current.Include(child));
            var data = await query.Where(o => o.Id == id && o.IsDeleted == false).FirstOrDefaultAsync();
            return data;
        }

        //public async Task<IEnumerable<TEntity>> Filter(params string[] includes)
        //{
        //    var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet, (current, child) => current.Include(child));
        //    return await query.Where(o => o.IsDeleted == false && o.InActive == false).ToListAsync();
        //}

        //public async Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate)
        //{
        //    var data = await EntitySet.Where(o => o.IsDeleted == false).Where(predicate).ToListAsync();
        //    return data;
        //}

        //public async Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        //{
        //    var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet, (current, child) => current.Include(child));
        //    var data = await query.Where(predicate).ToListAsync();
        //    return data;
        //}

        //public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 20, params string[] includes)
        //{
        //    var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet, (current, child) => current.Include(child));
        //    var skipCount = index * size;
        //    var resetSet = filter != null ? query.Where(filter).AsQueryable() : query.AsQueryable();
        //    resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
        //    total = resetSet.Count();
        //    return resetSet;
        //}

        //public async Task<IEnumerable<TEntity>> Query(Expression<Func<TEntity, bool>> filter, string children)
        //{
        //    return await EntitySet.Include(children).Where(filter).ToListAsync();
        //}

        #endregion

        #region Extra Functions

        public async Task<ApiObjectData> Get<T>(long id) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = await Find(id);
                data.ReturnData = _mapper.Map<T>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Get<T>(long id, params string[] includes) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = await Filter(id, includes);
                data.ReturnData = _mapper.Map<T>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        //public async Task<ApiObjectData> GetFirst<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = (await Filter(predicate)).FirstOrDefault();
        //        data.ReturnData = _mapper.Map<T>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> GetLast<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = (await Filter(predicate)).LastOrDefault();
        //        data.ReturnData = _mapper.Map<T>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> Get<T>() where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = await Find();
        //        data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> Get<T>(params string[] includes) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = await Filter(includes);
        //        data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = await Filter(predicate);
        //        data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate, params string[] includes) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = await Filter(predicate, includes);
        //        data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        public async Task<ApiObjectData> Save<T>(BaseEntityLongVm entityVm) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var entity = new TEntity();
                if (entityVm.Id != 0)
                {
                    entity = await Find(entityVm.Id);
                }
                entity = _mapper.Map(entityVm, entity);
                if (entityVm.Id != 0)
                {
                    entity.ModifiedDate = DateTime.Now;
                    entity.ModifiedBy = entityVm.LoggedInUserId;
                    UpdateColumnsStatus(entity, new[] { "CreatedBy", "CreatedDate", "IsDeleted", "InActive", }, false);
                }
                else
                {
                    entity.CreatedDate = DateTime.Now;
                    entity.CreatedBy = entityVm.LoggedInUserId;
                }
                if (await Save(entity))
                {
                    data.ReturnData = _mapper.Map<T>(entity);
                    data.Message.MsgSuccess();
                }
            }
            catch (Exception ex)
            {
                data.ReturnData = entityVm;
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Save<T>(BaseEntityLongVm entityVm, params string[] includes) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var entity = new TEntity();
                if (entityVm.Id != 0)
                {
                    entity = await Find(entityVm.Id);
                }
                entity = _mapper.Map(entityVm, entity);
                if (entityVm.Id != 0)
                {
                    entity.ModifiedDate = DateTime.Now;
                    entity.ModifiedBy = entityVm.LoggedInUserId;
                    UpdateColumnsStatus(entity, new[] { "CreatedBy", "CreatedDate", "IsDeleted", "InActive" }, false);
                }
                else
                {
                    entity.CreatedDate = DateTime.Now;
                    entity.CreatedBy = entityVm.LoggedInUserId;
                }
                if (await Save(entity))
                {
                    entity = await Filter(entity.Id, includes);
                    data.ReturnData = _mapper.Map<T>(entity);
                    data.Message.MsgSuccess();
                }
            }
            catch (Exception ex)
            {
                data.ReturnData = entityVm;
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Remove(long id)
        {
            var data = new ApiObjectData();
            try
            {
                var entity = await Find(id);
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    entity.ModifiedDate = DateTime.Now;
                    UpdateColumnsStatus(entity, new[] { "CreatedBy", "CreatedDate", "InActive" }, false);
                    await Save();
                    data.Message.MsgSuccess(_localizer["DeleteMsgSuccess"]);
                }
                else
                    data.Message.MsgError(_localizer["UnexpectedError"]);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Delete(long id)
        {
            var data = new ApiObjectData();
            try
            {
                var entity = await Find(id);
                if (entity != null)
                {
                    Delete(entity);
                    await Save();
                    data.Message.MsgSuccess(_localizer["DeleteMsgSuccess"]);
                }
                else
                    data.Message.MsgError(_localizer["UnexpectedError"]);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;

        }




        #endregion

        #endregion
    }

    public class BaseRepositoryGuid<TEntity> : BaseRepository<TEntity>, IBaseRepositoryGuid<TEntity>
        where TEntity : BaseEntityGuid, new()
    {
        #region Private Members

        #endregion

        #region Protected Members

        #endregion

        #region Public Members
        #endregion

        #region Contsructors

        protected BaseRepositoryGuid(DbContext context, IMapper mapper, IStringLocalizer<SharedResource> localizer) : base(context, mapper, localizer)
        {
        }

        #endregion

        #region Private Functions

        #endregion

        #region Public Functions

        #region Base Functions

        public override async Task<bool> Save(TEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                Add(entity);
            }
            return await Save();
        }

        public async void MarkAsDelete(Guid id)
        {
            (await Find(id)).IsDeleted = true;
        }

        //public T Map<T>(TEntity entity) where T : class
        //{
        //    return _mapper.Map<T>(entity);
        //}

        public async Task<TEntity> Find(Guid id)
        {
            return await EntitySet.Where(o => o.Id == id && o.IsDeleted == false).FirstOrDefaultAsync();
        }

        //public async Task<IEnumerable<TEntity>> Find()
        //{
        //    return await EntitySet.Where(o => o.IsDeleted == false).ToListAsync();
        //}

        public async Task<TEntity> Filter(Guid id, params string[] includes)
        {
            var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet, (current, child) => current.Include(child));
            var data = await query.Where(o => o.Id == id && o.IsDeleted == false).FirstOrDefaultAsync();
            return data;
        }

        //public async Task<IEnumerable<TEntity>> Filter(params string[] includes)
        //{
        //    var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet, (current, child) => current.Include(child));
        //    return await query.Where(o => o.IsDeleted == false && o.InActive == false).ToListAsync();
        //}

        //public async Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate)
        //{
        //    var data = await EntitySet.Where(o => o.IsDeleted == false).Where(predicate).ToListAsync();
        //    return data;
        //}

        //public async Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        //{
        //    var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet, (current, child) => current.Include(child));
        //    var data = await query.Where(predicate).ToListAsync();
        //    return data;
        //}

        //public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 20, params string[] includes)
        //{
        //    var query = includes.Aggregate<string, IQueryable<TEntity>>(EntitySet, (current, child) => current.Include(child));
        //    var skipCount = index * size;
        //    var resetSet = filter != null ? query.Where(filter).AsQueryable() : query.AsQueryable();
        //    resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
        //    total = resetSet.Count();
        //    return resetSet;
        //}

        //public async Task<IEnumerable<TEntity>> Query(Expression<Func<TEntity, bool>> filter, string children)
        //{
        //    return await EntitySet.Include(children).Where(filter).ToListAsync();
        //}

        #endregion

        #region Extra Functions

        public async Task<ApiObjectData> Get<T>(Guid id) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = await Find(id);
                data.ReturnData = _mapper.Map<T>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Get<T>(Guid id, params string[] includes) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var result = await Filter(id, includes);
                data.ReturnData = _mapper.Map<T>(result);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        //public async Task<ApiObjectData> GetFirst<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = (await Filter(predicate)).FirstOrDefault();
        //        data.ReturnData = _mapper.Map<T>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> GetLast<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = (await Filter(predicate)).LastOrDefault();
        //        data.ReturnData = _mapper.Map<T>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> Get<T>() where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = await Find();
        //        data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> Get<T>(params string[] includes) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = await Filter(includes);
        //        data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = await Filter(predicate);
        //        data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        //public async Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate, params string[] includes) where T : class
        //{
        //    var data = new ApiObjectData();
        //    try
        //    {
        //        var result = await Filter(predicate, includes);
        //        data.ReturnData = _mapper.Map<IEnumerable<T>>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        data.Message.MsgLogError(ex.ExceptionMessage());
        //    }
        //    return data;
        //}

        public async Task<ApiObjectData> Save<T>(BaseEntityGuidVm entityVm) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var entity = new TEntity();
                if (entityVm.Id != Guid.Empty)
                {
                    entity = await Find(entityVm.Id);
                }
                entity = _mapper.Map(entityVm, entity);
                if (entityVm.Id != Guid.Empty)
                {
                    entity.ModifiedDate = DateTime.Now;
                    entity.ModifiedBy = entityVm.LoggedInUserId;
                    UpdateColumnsStatus(entity, new[] { "CreatedBy", "CreatedDate", "IsDeleted", "InActive", }, false);
                }
                else
                {
                    entity.CreatedDate = DateTime.Now;
                    entity.CreatedBy = entityVm.LoggedInUserId;
                }
                if (await Save(entity))
                {
                    data.ReturnData = _mapper.Map<T>(entity);
                    data.Message.MsgSuccess();
                }
            }
            catch (Exception ex)
            {
                data.ReturnData = entityVm;
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Save<T>(BaseEntityGuidVm entityVm, params string[] includes) where T : class
        {
            var data = new ApiObjectData();
            try
            {
                var entity = new TEntity();
                if (entityVm.Id != Guid.Empty)
                {
                    entity = await Find(entityVm.Id);
                }
                entity = _mapper.Map(entityVm, entity);
                if (entityVm.Id != Guid.Empty)
                {
                    entity.ModifiedDate = DateTime.Now;
                    entity.ModifiedBy = entityVm.LoggedInUserId;
                    UpdateColumnsStatus(entity, new[] { "CreatedBy", "CreatedDate", "IsDeleted", "InActive" }, false);
                }
                else
                {
                    entity.CreatedDate = DateTime.Now;
                    entity.CreatedBy = entityVm.LoggedInUserId;
                }
                if (await Save(entity))
                {
                    entity = await Filter(entity.Id, includes);
                    data.ReturnData = _mapper.Map<T>(entity);
                    data.Message.MsgSuccess();
                }
            }
            catch (Exception ex)
            {
                data.ReturnData = entityVm;
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Remove(Guid id)
        {
            var data = new ApiObjectData();
            try
            {
                var entity = await Find(id);
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    entity.ModifiedDate = DateTime.Now;
                    UpdateColumnsStatus(entity, new[] { "CreatedBy", "CreatedDate", "InActive" }, false);
                    await Save();
                    data.Message.MsgSuccess(_localizer["DeleteMsgSuccess"]);
                }
                else
                    data.Message.MsgError(_localizer["UnexpectedError"]);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;
        }

        public async Task<ApiObjectData> Delete(Guid id)
        {
            var data = new ApiObjectData();
            try
            {
                var entity = await Find(id);
                if (entity != null)
                {
                    Delete(entity);
                    await Save();
                    data.Message.MsgSuccess(_localizer["DeleteMsgSuccess"]);
                }
                else
                    data.Message.MsgError(_localizer["UnexpectedError"]);
            }
            catch (Exception ex)
            {
                data.Message.MsgLogError(ex.ExceptionMessage());
            }
            return data;

        }

        #endregion

        #endregion
    }

    class ParameterUpdateVisitor : ExpressionVisitor
    {
        private ParameterExpression _oldParameter;
        private ParameterExpression _newParameter;

        public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (ReferenceEquals(node, _oldParameter))
                return _newParameter;

            return base.VisitParameter(node);
        }
    }
}
