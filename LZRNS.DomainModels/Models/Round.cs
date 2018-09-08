using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.Models
{
    public class Round : AbstractModel
    {
        [Required]
        public string RoundName { get; set; }

        public virtual List<Game> Games { get; set; }
    }
}
