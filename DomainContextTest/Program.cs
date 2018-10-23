using System.IO;
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
