using System.Collections.Generic;

namespace LZRNS.Common.Comparers
{
    public class TextComparer : ITextComparer
    {
        public string GetMostSimilar(IEnumerable<string> from, string to)
        {
            return to;
        }
    }
}