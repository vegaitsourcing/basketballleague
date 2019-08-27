using System.ComponentModel.DataAnnotations;
namespace LZRNS.Models.DocumentTypes.Nodes.Items
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "Obavezno polje.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Obavezno polje.")]
        [MaxLength(500, ErrorMessage = "Poruka ne može biti duža od 500 karaktera.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Obavezno polje.")]
        [EmailAddress(ErrorMessage ="Morate uneti validnu email adresu.")]
        public string Email { get; set; }
    }
}
