
using System;
using CommandLine;
using DotNetFs;
using DotNetFs.Io;

using DirectoryInfo = System.IO.DirectoryInfo;

namespace ReadPath
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts => Processing(opts));

        }

        static void Processing(Options opts)
        {
            if (!opts.Anything)
                opts.Everything = true;

            foreach (var path in opts.Paths)
            {
                Console.WriteLine($"Input: {path.Quoted()}");

                if (opts.Normalized)
                {
                    string normalized = Path.GetNormalized(path);
                    Console.WriteLine($"Path.GetNormalized: {normalized.Quoted()}");
                }

                if (opts.AnyParentOption)
                {
                    string parentName = Path.GetParentName(path);
                    if (opts.ParentName)
                        Console.WriteLine($"Path.GetParentName: {parentName.Quoted()}");

                    if (opts.NormalizedParentName)
                    {
                        var tag = (opts.ParentName)
                            ? "(+ GetNormalized)"
                            : "Path.GetParentName+GetNormalized";
                        string parentNormalized = Path.GetNormalized(parentName);
                        Console.WriteLine($"{tag}: {parentNormalized.Quoted()}");
                    }
                }

                if (opts.Directory)
                {
                    string directory;
                    try
                    {
                        var dirInfo = Directory.GetDirectory(path);
                        directory = $"Exists: [{(dirInfo.Exists ? "YES" : "NO")}], {dirInfo.FullName.Quoted()}";
                    }
                    catch (ArgumentException e)
                    {
                        directory = e.Message;
                    }
                    Console.WriteLine($"Directory.GetDirectory: {directory}");
                }
                Console.WriteLine();
            }
        }
    }
}
