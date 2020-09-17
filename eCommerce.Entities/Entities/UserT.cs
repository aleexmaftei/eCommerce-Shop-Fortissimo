using eCommerce.Common;
using System.Collections.Generic;

namespace eCommerce.DataAccess
{
    public partial class UserT : IEntity
    {
        public UserT()
        {
            Cart = new HashSet<Cart>();
            UserBuyHistory = new HashSet<UserBuyHistory>();
            UserRole = new HashSet<UserRole>();
        }

        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string AddressDetail { get; set; }
        public int PostalCode { get; set; }

        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<UserBuyHistory> UserBuyHistory { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}