using System.Buffers;
using System.Runtime.CompilerServices;
namespace Algo
{
    public class StringCompression
    {
        public static class Settings
        {
            /// <summary>
            /// MemoryPool.Shared.Rent() will be used if lenght of text is greater or eq. Default value 8K. 
            /// </summary>
            public static int MemoryStringPoolRentThreshold = 8 * 1024;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Compress(string input) => Compress(input.AsMemory()).ToString();
        #region Compress 
        public static ReadOnlyMemory<char> Compress(ReadOnlyMemory<char> input)
        {
            if (input.IsEmpty) return ReadOnlyMemory<char>.Empty;
            var estimatedSize = CalculateCompressedSize(input.Span);
            using var pool = estimatedSize >= Settings.MemoryStringPoolRentThreshold ? MemoryPool<char>.Shared.Rent(estimatedSize) : null;
            var buffer = pool?.Memory ?? new Memory<char>(new char[estimatedSize]);
            var writePosition = Compression(input.Span, buffer.Span);
            return buffer[..writePosition];
        }
        private static int Compression(ReadOnlySpan<char> input, Span<char> output)
        {
            var writePosition = 0;
            var current = input[0];
            var count = 1;
            for (var i = 1; i < input.Length; i++)
            {
                if (input[i] == current)
                {
                    count++;
                    continue;
                }
                writePosition = WriteCompressed(output, writePosition, current, count);
                current = input[i];
                count = 1;
            }
            return WriteCompressed(output, writePosition, current, count);
        }
        private static int WriteCompressed(Span<char> buffer, int position, char c, int count)
        {
            buffer[position++] = c;
            if (count > 1)
            {
                if (!count.TryFormat(buffer.Slice(position), out int written)) throw new InvalidOperationException("No space in buffer to set number in test form");
                position += written;
            }
            return position;
        }
        private static int CalculateCompressedSize(ReadOnlySpan<char> input)
        {
            if (input.IsEmpty) return 0;
            var size = 0;
            var current = input[0];
            var count = 1;
            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == current)
                {
                    count++;
                    continue;
                }
                size += 1 + (count > 1 ? count.ToString().Length : 0);
                current = input[i];
                count = 1;
            }
            size += 1 + (count > 1 ? count.ToString().Length : 0);
            return Math.Min(size, input.Length);
        }
        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Decompress(string input) => Decompress(input.AsMemory()).ToString();
        #region Decompress
        public static ReadOnlyMemory<char> Decompress(ReadOnlyMemory<char> input)
        {
            if (input.IsEmpty) return ReadOnlyMemory<char>.Empty;
            int estimatedSize = CalculateDecompressedSize(input.Span);
            using var pool = estimatedSize >= Settings.MemoryStringPoolRentThreshold ? MemoryPool<char>.Shared.Rent(estimatedSize) : null;
            var buffer = pool?.Memory ?? new Memory<char>(new char[estimatedSize]);
            int writePos = Decompression(input.Span, buffer.Span);
            return buffer[..writePos];
        }
        private static int Decompression(ReadOnlySpan<char> compressed, Span<char> output)
        {
            var readPos = 0;
            var writePos = 0;
            while (readPos < compressed.Length)
            {
                var c = compressed[readPos++];
                var numStart = readPos;
                while (readPos < compressed.Length && char.IsDigit(compressed[readPos])) readPos++;
                var count = readPos > numStart ? int.Parse(compressed[numStart..readPos]) : 1;
                output.Slice(writePos, count).Fill(c);
                writePos += count;
            }
            return writePos;
        }
        private static int CalculateDecompressedSize(ReadOnlySpan<char> compressed)
        {
            var size = 0;
            var i = 0;
            while (i < compressed.Length)
            {
                if (i + 1 < compressed.Length && char.IsDigit(compressed[i + 1]))
                {
                    var numStart = i + 1;
                    while (numStart < compressed.Length && char.IsDigit(compressed[numStart])) numStart++;
                    size += int.Parse(compressed.Slice(i + 1, numStart - i - 1));
                    i = numStart;
                }
                else
                {
                    size++;
                    i++;
                }
            }
            return size;
        }
        #endregion
    }
}
