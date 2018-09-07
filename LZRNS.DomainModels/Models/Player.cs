using System;
using System.Collections.Generic;
using System.Text;

namespace LZRNS.DomainModel.Models
{
    public class Player: AbstractModel
    {
        public string Name { get; set; }

        public string MiddleName { get; set; }
        
        public string LastName { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public int YearOfBirth { get; set; }
    }
}
