using System.ComponentModel.DataAnnotations;

namespace ContactsApi.Models
{
    public class Contact
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        [StringLength(30)]
        public string Company { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.ImageUrl)]
        public string ProfileImage { get; set; }
        [Required]
        public DateOnly BirthDate { get; set; }

    }
}
