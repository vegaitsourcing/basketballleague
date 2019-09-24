using System.Collections.Generic;

namespace LZRNS.Common.Comparers
{
    public interface ITextComparer
    {
        string GetMostSimilar(IEnumerable<string> from, string to);
    }
}