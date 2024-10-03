using System;

namespace Resturant.Core
{
    public class BaseEntity
    {
        public virtual long CreatedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual long? ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual bool InActive { get; set; }
    }

    public class BaseEntityLong : BaseEntity
    {
        public virtual long Id { get; set; }
    }

    public class BaseEntityGuid : BaseEntity
    {
        public virtual Guid Id { get; set; }
    }
}
