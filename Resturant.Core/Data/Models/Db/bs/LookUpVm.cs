namespace Resturant.Core
{
    public class LookUpVm: BaseEntityLongVm
    {
        public long ParentId { get; set; }

        public string? NameAr { get; set; }

        public string? NameEn { get; set; }

        public string? Description { get; set; }
    }

    public class LookUpDto : BaseEntityLongVm
    {
        public long ParentId { get; set; }

        public string? NameAr { get; set; }

        public string? NameEn { get; set; }

        public string? Description { get; set; }
    }
}
