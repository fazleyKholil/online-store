using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using OnlineStore.Legacy.DataAccess;

namespace Legacy.Benchmark
{
    [KeepBenchmarkFiles]
    [MarkdownExporterAttribute.GitHub]
    [RPlotExporter]
    public class LedgerDbTest
    {
        private ILedgerDb _ledgerDb;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _ledgerDb = new InMemoryLedgerDb(null);
        }


        [Benchmark]
        public async Task UpdateLedger()
        {
            var threadCount = Environment.ProcessorCount;
            var threads = new List<Thread>();
            for (var i = 0; i < threadCount; i++)
            {
                var thread = new Thread(async obj => { await _ledgerDb.UpdateLedger(100, 100, 10, 10); }
                );
                thread.Start();
                threads.Add(thread);
            }

            foreach (var t in threads) t.Join();
        }

        [Benchmark]
        public async Task UpdateLedgerOptimised()
        {
            var threadCount = Environment.ProcessorCount;
            var threads = new List<Thread>();
            for (var i = 0; i < threadCount; i++)
            {
                var thread = new Thread(async obj => { await _ledgerDb.UpdateLedgerOptimised(100, 100, 10, 10); }
                );
                thread.Start();
                threads.Add(thread);
            }

            foreach (var t in threads) t.Join();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
        }
    }
}