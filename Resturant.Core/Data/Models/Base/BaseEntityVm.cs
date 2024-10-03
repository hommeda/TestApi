using System;

namespace Resturant.Core
{
    public class BaseEntityVm
    {
        public virtual long LoggedInUserId { get; set; }
        public virtual string? LoggedInUserName { get; set; }
        public virtual string? PcName { get; set; }
        public virtual string? PcIp { get; set; }
    }

    public class BaseEntityLongVm : BaseEntityVm
    {
        public virtual long Id { get; set; }
    }

    public class BaseEntityGuidVm : BaseEntityVm
    {
        public virtual Guid Id { get; set; }
    }
}
