using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.Common.Comparers
{
    public class TextComparer : ITextComparer
    {
        public string GetMostSimilar(IEnumerable<string> from, string to)
        {
            int minDiff = int.MaxValue;
            string mostSimilar = string.Empty;
            foreach (string str in from)
            {
                int currDiff = LevenshteinDistance.Compute(str, to);
                if (currDiff < minDiff)
                {
                    minDiff = currDiff;
                    mostSimilar = str;
                }
            }

            return mostSimilar;
        }
    }
}