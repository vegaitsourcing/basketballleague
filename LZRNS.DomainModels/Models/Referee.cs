using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LZRNS.DomainModel.Models
{
    public class Referee : AbstractModel
    {
        [Required]
        [DisplayName("Ime")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Prezime")]
        public string LastName { get; set; }
    }
}
