using Algo;
namespace SimpleStringCommpression
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Text:");
            var input = Console.ReadLine();
            var compressed = StringCompression.Compress(input);
            Console.WriteLine(compressed);
            var decompressed = StringCompression.Decompress(compressed);
            Console.WriteLine(decompressed);
            Console.WriteLine($"Decompression(Compression(input)) {(input == decompressed ? "" : "!")}= original");
            Console.ReadKey();
        }
    }
}
