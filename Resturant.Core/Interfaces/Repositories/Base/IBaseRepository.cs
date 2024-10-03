using System.Linq.Expressions;
using static Resturant.Core.Utilities.Enums;

namespace Resturant.Core
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entity);
        Task<bool> Save();
        Task<bool> Save(TEntity entity);
        void Refresh(TEntity entity);
        void Delete(TEntity entity);
        T Map<T>(TEntity entity) where T : class;
        Task<TEntity> First(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> First(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        Task<TEntity> Last(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> Find();
        Task<IEnumerable<TEntity>> Filter(params string[] includes);
        Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 20, params string[] includes);
        Task<IEnumerable<TEntity>> Query(Expression<Func<TEntity, bool>> filter, string children);
        Task<ApiObjectData> GetFirst<T>(Expression<Func<TEntity, bool>> predicate) where T : class;
        Task<ApiObjectData> GetLast<T>(Expression<Func<TEntity, bool>> predicate) where T : class;
        Task<ApiObjectData> Get<T>() where T : class;
        Task<ApiObjectData> Get<T>(params string[] includes) where T : class;
        Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate) where T : class;
        Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderby, OrderType orderType) where T : class;
        Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate, params string[] includes) where T : class;
        Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderby, OrderType orderType, params string[] includes) where T : class;
        Task<ApiObjectData> Get<T>(Expression<Func<TEntity, object>> predicate) where T : class;

        //void MarkAsDelete(long id);
        //Task<TEntity> Find(long id);
        //Task<TEntity> Filter(long id, params string[] includes);
        //Task<ApiObjectData> Get<T>(long id) where T : class;
        //Task<ApiObjectData> Save<T>(BaseEntityVm entityVm) where T : class;
        //Task<ApiObjectData> Save<T>(BaseEntityVm entityVm, params string[] includes) where T : class;
    }

    public interface IBaseRepositoryLong<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntityLong
    {
        //void Add(TEntity entity);
        //void AddRange(IEnumerable<TEntity> entity);
        //Task<bool> Save();
        //Task<bool> Save(TEntity entity);
        //void Delete(TEntity entity);
        //T Map<T>(TEntity entity) where T : class;
        //Task<IEnumerable<TEntity>> Find();
        //Task<IEnumerable<TEntity>> Filter(params string[] includes);
        //Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate);
        //Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        //IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 20, params string[] includes);
        //Task<IEnumerable<TEntity>> Query(Expression<Func<TEntity, bool>> filter, string children);
        //Task<ApiObjectData> Get<T>() where T : class;
        //Task<ApiObjectData> Get<T>(params string[] includes) where T : class;
        //Task<ApiObjectData> GetFirst<T>(Expression<Func<TEntity, bool>> predicate) where T : class;
        //Task<ApiObjectData> GetLast<T>(Expression<Func<TEntity, bool>> predicate) where T : class;
        //Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate) where T : class;
        //Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate, params string[] includes) where T : class;

        void MarkAsDelete(long id);
        Task<TEntity> Find(long id);
        Task<TEntity> Filter(long id, params string[] includes);
        Task<ApiObjectData> Get<T>(long id) where T : class;
        Task<ApiObjectData> Get<T>(long id, params string[] includes) where T : class;
        Task<ApiObjectData> Save<T>(BaseEntityLongVm entityVm) where T : class;
        Task<ApiObjectData> Save<T>(BaseEntityLongVm entityVm, params string[] includes) where T : class;
    }

    public interface IBaseRepositoryGuid<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntityGuid
    {
        //void Add(TEntity entity);
        //void AddRange(IEnumerable<TEntity> entity);
        //Task<bool> Save();
        //Task<bool> Save(TEntity entity);
        //void Delete(TEntity entity);
        //T Map<T>(TEntity entity) where T : class;
        //Task<IEnumerable<TEntity>> Find();
        //Task<IEnumerable<TEntity>> Filter(params string[] includes);
        //Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate);
        //Task<IEnumerable<TEntity>> Filter(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        //IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 20, params string[] includes);
        //Task<IEnumerable<TEntity>> Query(Expression<Func<TEntity, bool>> filter, string children);
        //Task<ApiObjectData> Get<T>() where T : class;
        //Task<ApiObjectData> Get<T>(params string[] includes) where T : class;
        //Task<ApiObjectData> GetFirst<T>(Expression<Func<TEntity, bool>> predicate) where T : class;
        //Task<ApiObjectData> GetLast<T>(Expression<Func<TEntity, bool>> predicate) where T : class;
        //Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate) where T : class;
        //Task<ApiObjectData> Get<T>(Expression<Func<TEntity, bool>> predicate, params string[] includes) where T : class;
        void MarkAsDelete(Guid id);
        Task<TEntity> Find(Guid id);
        Task<TEntity> Filter(Guid id, params string[] includes);
        Task<ApiObjectData> Get<T>(Guid id) where T : class;
        Task<ApiObjectData> Get<T>(Guid id, params string[] includes) where T : class;
        Task<ApiObjectData> Save<T>(BaseEntityGuidVm entityVm) where T : class;
        Task<ApiObjectData> Save<T>(BaseEntityGuidVm entityVm, params string[] includes) where T : class;
    }
}
