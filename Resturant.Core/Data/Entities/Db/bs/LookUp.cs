namespace Resturant.Core;

public partial class LookUp
{

    public long ParentId { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

    public string? Description { get; set; }


    public virtual ICollection<Account>? AccountAccountNatureNavigations { get; set; }

    public virtual ICollection<Account>? AccountAccountTypeNavigations { get; set; }

    public virtual ICollection<DailyEntry>? DailyEntryDocumentTypes { get; set; }

    public virtual ICollection<DailyEntry>? DailyEntryEntryTypes { get; set; }

    public virtual ICollection<Order>? Orders { get; set; }

    public virtual ICollection<Person>? PersonGenders { get; set; }

    public virtual ICollection<Person>? PersonPersonTypes { get; set; }
    public virtual ICollection<StoreTransaction>? StoreTransactionActionTypes { get; set; }

    public virtual ICollection<StoreTransaction>? StoreTransactionDocumnetTypes { get; set; }

}
