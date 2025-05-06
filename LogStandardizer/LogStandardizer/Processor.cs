using System.Globalization;
using System.Text.RegularExpressions;
namespace LogStandardizer
{
    public class Processor
    {
        private static readonly Dictionary<string, string> LogLevelMap = new(StringComparer.OrdinalIgnoreCase)
        {
            ["INFORMATION"] = "INFO",
            ["WARNING"] = "WARN",
            ["INFO"] = "INFO",
            ["WARN"] = "WARN",
            ["ERROR"] = "ERROR",
            ["ERR"] = "ERROR",
            ["DEBUG"] = "DEBUG",
            ["DBG"] = "DEBUG"
        };
        private static readonly Regex LogFormatRegex = new(
            @"^(\d{2}\.\d{2}\.\d{4})\s(\d+:\d+:\d+\.\d+)\s+([A-Z]+)\s+(.*)|^(\d{4}-\d{2}-\d{2})\s(\d+:\d+:\d+\.\d+)\|([^|]+)\|.*\|([^|]+)\|(.*)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public async Task ProcessAsync(string inputPath, string outputPath)
        {
            if (string.IsNullOrWhiteSpace(inputPath)) throw new ArgumentNullException(nameof(inputPath));
            if (string.IsNullOrWhiteSpace(outputPath)) throw new ArgumentNullException(nameof(outputPath));
            var attributes = File.GetAttributes(inputPath);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                await ProcessDirectoryAsync(inputPath, outputPath);
                return;
            }
            await ProcessFileAsync(inputPath, Path.Combine(outputPath, "standardized.log"));
        }
        private async Task ProcessDirectoryAsync(string inputDir, string outputDir)
        {
            Directory.CreateDirectory(outputDir);
            var files = Directory.EnumerateFiles(inputDir, "*.log", SearchOption.AllDirectories);
            await Parallel.ForEachAsync(files, async (file, ct) =>
            {
                var outputFile = Path.Combine(outputDir, Path.GetFileName(file));
                await ProcessFileAsync(file, outputFile);
            });
        }
        private async Task ProcessFileAsync(string inputFile, string outputFile)
        {
            await using var writer = new StreamWriter(outputFile);
            await using var problemWriter = new StreamWriter(Path.ChangeExtension(outputFile, ".problems.log"));
            await foreach (var line in File.ReadLines(inputFile).ToAsyncEnumerable())// avoid reading all in mem
            {
                if (TryParseLine(line, out var logEntry))
                {
                    await writer.WriteLineAsync($"{logEntry.Date:dd-MM-yyyy}\t{logEntry.Time}\t{logEntry.LogLevel}\t{logEntry.CallerMethod}\t{logEntry.Message}");
                }
                else
                {
                    await problemWriter.WriteLineAsync(line);
                }
            }
        }
        private bool TryParseLine(string line, out LogEntry logEntry)
        {
            logEntry = null;
            var match = LogFormatRegex.Match(line);
            if (!match.Success) return false;
            var dateStr = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[5].Value;
            var timeStr = match.Groups[2].Success ? match.Groups[2].Value : match.Groups[6].Value;
            var logLevel = match.Groups[3].Success ? match.Groups[3].Value : match.Groups[7].Value;
            var callerMethod = match.Groups[8].Success ? match.Groups[8].Value : null;
            var message = match.Groups[4].Success ? match.Groups[4].Value : match.Groups[9].Value;
            dateStr = dateStr.Trim();
            logLevel = logLevel.Trim();
            if (!DateTime.TryParseExact(dateStr, ["dd.MM.yyyy", "yyyy-MM-dd"],
                                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)) return false;
            if (!LogLevelMap.TryGetValue(logLevel, out var mappedLevel)) return false;
            logEntry = new LogEntry
            {
                Date = date,
                Time = timeStr,
                LogLevel = mappedLevel,
                CallerMethod = callerMethod ?? "DEFAULT",
                Message = message.Trim()
            };
            return true;
        }
    }
}
