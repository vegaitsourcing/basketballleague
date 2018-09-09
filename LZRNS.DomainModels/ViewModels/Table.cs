using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.ViewModels
{
    public class Table
    {
        ICollection<Team> Teams
        {
            get
            {
                return Teams.OrderByDescending(t => t.Points).ToList();
            }
        }

        Season Season { get; set; }

        League League { get; set; }
    }
}
