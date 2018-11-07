
using System.Collections.Generic;

namespace LZRNS.Web.Helpers
{
    public static class RenderHelper
    {
        public static IEnumerable<string> GetAlphabet()
        {
            var letters = new string[] { "a", "b", "c", "č", "ć", "d", "dž", "đ", "e", "f", "g", "h", "i", "j", "k", "l", "lj", "m", "n", "nj", "o", "p", "r", "s", "š", "t", "u", "v", "z", "ž" };

            foreach (var c in letters)
            {
                yield return c;
            }
        }
    }
}