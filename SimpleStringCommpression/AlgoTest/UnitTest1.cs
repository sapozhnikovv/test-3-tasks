using Algo;
namespace AlgoTest
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("aa", "a2")]
        [InlineData("aaa", "a3")]
        [InlineData("aaabb", "a3b2")]
        [InlineData("aaabba", "a3b2a")]
        [InlineData("abc", "abc")]
        [InlineData("aaabbbcccddde", "a3b3c3d3e")]
        [InlineData("aaaaaaaaaa", "a10")]
        [InlineData("aaaaaaaaaabbb", "a10b3")]
        [InlineData("aabccbaa", "a2bc2ba2")]
        public void CompressDecompress(string original, string compressed)
        {
            var compressedResult = StringCompression.Compress(original);
            var decompressedResult = StringCompression.Decompress(compressedResult);
            Assert.Equal(compressed, compressedResult.ToString());
            Assert.Equal(original, decompressedResult.ToString());
        }

        [Fact]
        public void CompressLongString()
        {
            var longString = new string('a', 1000) + new string('b', 2000);
            var compressed = StringCompression.Compress(longString);
            var decompressed = StringCompression.Decompress(compressed);
            Assert.Equal("a1000b2000", compressed.ToString());
            Assert.Equal(longString, decompressed.ToString());
            longString += new string('c', 1000);
            compressed = StringCompression.Compress(longString);
            decompressed = StringCompression.Decompress(compressed);
            Assert.Equal("a1000b2000c1000", compressed.ToString());
            Assert.Equal(longString, decompressed.ToString());
        }

        [Fact]
        public void CompressWithPool()
        {
            StringCompression.Settings.MemoryStringPoolRentThreshold = 10;
            var testString = "aaabbbcccddd";
            var compressed = StringCompression.Compress(testString);
            var decompressed = StringCompression.Decompress(compressed);
            Assert.Equal("a3b3c3d3", compressed.ToString());
            Assert.Equal(testString, decompressed.ToString());
        }

        [Fact]
        public void CompressSingleCharsNoNumbersAdded()
        {
            var input = "abcdef";
            var result = StringCompression.Compress(input);
            Assert.Equal(input, result.ToString());
        }
    }
}