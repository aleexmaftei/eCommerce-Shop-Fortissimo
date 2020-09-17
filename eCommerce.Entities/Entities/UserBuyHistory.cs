using eCommerce.Common;
using System;

namespace eCommerce.DataAccess
{
    public partial class UserBuyHistory : IEntity
    {
        public int UserBuyHistoryId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int QuantityBuy { get; set; }
        public DateTime DateBuy { get; set; }

        public virtual Product Product { get; set; }
        public virtual UserT User { get; set; }
    }
}