using System;
using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using CommandLine;

namespace Legacy.Benchmark
{
    class Program
    {
        public class Options
        {
            [Option('d', "debug", Required = false, HelpText = "Set debug mode.")]
            public bool IsDebug { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    var benchmarkSwitcher = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly);

                    IConfig config = new BenchmarkConfig();

                    if (o.IsDebug)
                    {
                        config = new DebugInProcessConfig();

                        if (args.Length == 1)
                        {
                            benchmarkSwitcher.Run(new[] {"-f", "*"}, config);
                            return;
                        }
                    }

                    if (args.Any())
                    {
                        benchmarkSwitcher.Run(args, config);
                    }
                    else
                    {
                        benchmarkSwitcher.Run(new[] {"-f", "*"}, config);
                    }
                });
        }
    }
}