using LZRNS.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainContextTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = new BasketballDbContext();

            Console.WriteLine("DONE");
        }
    }
}
