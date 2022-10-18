using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;

namespace Legacy.Benchmark
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            // Disable InProcess
            // Add(Job.Default.With(InProcessEmitToolchain.Instance));
            Add(CsvMeasurementsExporter.Default);
            Add(RPlotExporter.Default);
            Add(DefaultConfig.Instance.GetAnalysers().ToArray());
            Add(DefaultConfig.Instance.GetColumnProviders().ToArray());
            Add(DefaultConfig.Instance.GetDiagnosers().ToArray());
            Add(DefaultConfig.Instance.GetExporters().ToArray());
            Add(DefaultConfig.Instance.GetFilters().ToArray());
            Add(DefaultConfig.Instance.GetJobs().ToArray());
            Add(DefaultConfig.Instance.GetHardwareCounters().ToArray());
            Add(DefaultConfig.Instance.GetLoggers().ToArray());
            Add(DefaultConfig.Instance.GetLogicalGroupRules().ToArray());
            Add(DefaultConfig.Instance.GetValidators().ToArray());
            Add(MemoryDiagnoser.Default);
        }
    }
}