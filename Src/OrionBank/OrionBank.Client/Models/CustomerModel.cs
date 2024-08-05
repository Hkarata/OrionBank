using System.ComponentModel.DataAnnotations;

namespace OrionBank.Client.Models
{
    public class CustomerModel
    {
        [Required]
        [Display(Name = "Customer's ID")]
        public string Id { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Customer's first name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Customer's last name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Customer's other name")]
        public string OtherName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Customer's email address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Customer's phone number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Customer's address")]
        public string Address { get; set; } = string.Empty;
    }
}
