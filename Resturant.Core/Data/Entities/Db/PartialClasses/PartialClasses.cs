namespace Resturant.Core
{
    #region Fn
    public partial class Account : BaseEntityLong { }
    public partial class DailyEntry : BaseEntityGuid { }
    public partial class AccountStatment : BaseEntityGuid { }
    public partial class FinancialPeriod : BaseEntityLong { }
    #endregion

    #region Bs
    public partial class LookUp : BaseEntityLong { }
    public partial class Place : BaseEntityLong { }
    #endregion

    #region SN
    public partial class Order : BaseEntityLong { }
    public partial class OrderDetail : BaseEntityLong { }

    public partial class Person : BaseEntityLong { }
         
    #endregion

    #region ST
    public partial class Category : BaseEntityLong { }
    public partial class Item : BaseEntityLong { }
    public partial class ItemHistory : BaseEntityLong { }
    public partial class ItemUnit : BaseEntityLong { }
    public partial class ItemOption : BaseEntityLong { }
    public partial class Store : BaseEntityLong { }
    public partial class StoreTransaction : BaseEntityGuid { }
    public partial class Unit : BaseEntityLong { }
    public partial class ItemPhoto : BaseEntityLong { }
    public partial class ItemProduction : BaseEntityLong { }

    #endregion

    #region SC
    public partial class Menu : BaseEntityLong { }
    public partial class Role : BaseEntityLong { }
    public partial class RoleMenu : BaseEntityLong  { }
    public partial class UserRole : BaseEntityLong  { }
    public partial class User : BaseEntityLong  { }
    public partial class Form : BaseEntityLong  { }
    public partial class RoleForm : BaseEntityLong  {  }
    public partial class LogAction : BaseEntityGuid { }
    #endregion
}
