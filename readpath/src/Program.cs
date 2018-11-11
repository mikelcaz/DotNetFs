using System;
using CommandLine;

namespace ReadPath
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opt => {
                    if(!opt.Anything)
                        opt.Everything = true;

                    Console.WriteLine(opt);
                });
        }
    }
}
