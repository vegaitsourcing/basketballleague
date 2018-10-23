using System.ComponentModel.DataAnnotations;

namespace LZRNS.DomainModel.Models
{
    public class Referee : AbstractModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
