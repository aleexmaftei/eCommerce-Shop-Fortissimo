using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.MyProfileVM
{
    public class DeliveryLocationVm
    {
        [Required(ErrorMessage = "Nume obligatoriu")]
        [DisplayName("Country")]
        public string CountryName { get; set; }

        [Required(ErrorMessage = "Regiune obligatoriu")]
        [DisplayName("Region")]
        public string RegionName { get; set; }

        [Required(ErrorMessage = "Oras obligatoriu")]
        [DisplayName("City")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Adresa obligatoriu")]
        [DisplayName("Adress")]
        public string AddressDetail { get; set; }

        [Required(ErrorMessage = "Cod Postal obligatoriu")]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
    }
}
