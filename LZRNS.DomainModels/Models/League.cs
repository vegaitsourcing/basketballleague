using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LZRNS.DomainModel.Models
{
    public class League: AbstractModel
    {
        [Required]
        public string Name { get; set; }

        public List<Season> Seasons { get; set; }
    }
}
