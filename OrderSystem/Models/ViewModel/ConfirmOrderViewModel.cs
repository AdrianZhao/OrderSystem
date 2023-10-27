using System.ComponentModel.DataAnnotations;
namespace OrderSystem.Models.ViewModel
{
    public class ConfirmOrderViewModel
    {
        public Cart Cart { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        public decimal ConvertedPrice { get; set; }
        public decimal TotalPrice { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Mailing Code is required")]
        public string MailingCode { get; set; }
    }
}
