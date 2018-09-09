using LZRNS.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableLoader.Converter;

namespace DomainContextTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var converter = new Converter();

            var text = System.IO.File.ReadAllBytes(@"1.txt");
            MemoryStream ms = new MemoryStream(text);
            converter.Convert(ms);
        }
    }
}
