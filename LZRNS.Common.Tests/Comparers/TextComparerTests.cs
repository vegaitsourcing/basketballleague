using LZRNS.Common.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LZRNS.Common.Comparers.Tests
{
    public class TextComparerTests
    {
        private readonly TextComparer _sut;
        private readonly List<string> _stringList = new List<string> { "apsolventi", "becej", "blejkersi", "celarevo" };

        public TextComparerTests()
        {
            _sut = new TextComparer();
        }

        [Theory]
        [InlineData("apsolventi", "apsolventi")]
        public void GetMostSimilar_InputExistsInCollection_ReturnInput(string input, string expected)
        {
            string actual = _sut.GetMostSimilar(_stringList, input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("apsol ven ti", "apsolventi")]
        public void GetMostSimilar_InputIsDifferentBySpaces_ReturnMostSimilar(string input, string expected)
        {
            string actual = _sut.GetMostSimilar(_stringList, input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("apsloventi", "apsolventi")]
        public void GetMostSimilar_InputIsMisspeled_ReturnMostSimilar(string input, string expected)
        {
            string actual = _sut.GetMostSimilar(_stringList, input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("bečej", "becej")]
        public void GetMostSimilar_InputIsHasSerbianLatinLetter_ReturnMostSimilar(string input, string expected)
        {
            string actual = _sut.GetMostSimilar(_stringList, input);
            Assert.Equal(expected, actual);
        }
    }
}