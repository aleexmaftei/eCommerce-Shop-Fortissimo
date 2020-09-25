using eCommerce.Common;

namespace eCommerce.DataAccess
{
    public partial class ProductComment : IEntity
    {
        public int ProductCommentId { get; set; }
        public int ProductId { get; set; }
        public string UserNameComment { get; set; }
        public int? ProductRating { get; set; }
        public string Comment { get; set; }

        public virtual Product Product { get; set; }
    }
}