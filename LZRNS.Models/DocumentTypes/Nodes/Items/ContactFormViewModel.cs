using System.ComponentModel.DataAnnotations;
namespace LZRNS.Models.DocumentTypes.Nodes.Items
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "This information is missing")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This information is missing")]
        [MaxLength(500, ErrorMessage = "Your  message must be 500 characters or less")]
        public string Message { get; set; }

        [Required(ErrorMessage = "This information is missing")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
