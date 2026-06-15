using System;
using System.IO;
using System.Threading.Tasks;
using CSharpApiExtractor;

namespace CSharpApiExtractor.Example
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            string defaultSourcePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "CSharpApiExtractor"));
            string sourcePath = defaultSourcePath;

            ExtractorOptions options = new ExtractorOptions();

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                if (string.Equals(arg, "--exclude", StringComparison.OrdinalIgnoreCase))
                {
                    if (i + 1 >= args.Length)
                    {
                        throw new ArgumentException("Expected a path after --exclude.");
                    }

                    options.ExcludedPaths.Add(Path.GetFullPath(args[++i]));
                    continue;
                }

                if (sourcePath == defaultSourcePath)
                {
                    sourcePath = Path.GetFullPath(arg);
                    continue;
                }

                throw new ArgumentException($"Unknown argument: {arg}");
            }

            options.SourcePaths.Add(sourcePath);
            options.ExcludedPaths.Add(Path.Combine(sourcePath, "Editor"));

            Extractor extractor = new Extractor(options);
            ApiDocument document = await extractor.ExtractAsync();

            Console.WriteLine(document.ToJsonString(pretty: true));
            if (document.MissedItems.Count > 0)
            {
                Console.WriteLine("Missed items:");
                foreach (string item in document.MissedItems)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
