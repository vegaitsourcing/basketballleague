using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.TimetableModels
{
    public class SingleGameModel: AbstractModel
    {
        public string Season { get; set; }

        public string Liga { get; set; }

        public string Date { get; set; }
        
        public string Time { get; set; }

        public string TeamA { get; set; }

        public string TeamB { get; set; }
    }
}
