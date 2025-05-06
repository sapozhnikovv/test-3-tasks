namespace LogStandardizer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Encoding of files should be UTF8");
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: <inputPath> <outputPath>");
                args = new string[2];
                Console.WriteLine("inputPath:");
                args[0] = Console.ReadLine();
                Console.WriteLine("outputPath:");
                args[1] = Console.ReadLine();
                /*
#if DEBUG 
                if (string.IsNullOrEmpty(args[0])) args[0] = "..\\..\\..\\files";
                if (string.IsNullOrEmpty(args[1])) args[1] = "..\\..\\..\\files\\output";
#endif
                */
            }
            Console.WriteLine($"Starting... from ${args[0]}, to ${args[1]}");
            var processor = new Processor();
            await processor.ProcessAsync(args[0], args[1]);
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
