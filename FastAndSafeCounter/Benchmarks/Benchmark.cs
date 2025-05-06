using BenchmarkDotNet.Attributes;
namespace ServerBenchmark
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        private const int ThreadCount = 64;
        private const int Iterations = 1000;

        [Benchmark]
        public void NormalServer_ConcurrentReadsNoWrites()
        {
            Parallel.For(0, ThreadCount, _ =>
            {
                for (int i = 0; i < Iterations; i++) NormalServer.NormalServer.GetCount();
            });
        }

        [Benchmark]
        public void NormalServer_ConcurrentWritesNoReads()
        {
            Parallel.For(0, ThreadCount, _ =>
            {
                for (int i = 0; i < Iterations; i++) NormalServer.NormalServer.AddToCount(1);
            });
        }

        [Benchmark]
        public void NormalServer_ReadAndWrite()
        {
            Parallel.Invoke(NormalServer_ConcurrentWritesNoReads, NormalServer_ConcurrentReadsNoWrites);
        }

        [Benchmark]
        public void HighLoadServer_ConcurrentReadsNoWrites()
        {
            Parallel.For(0, ThreadCount, _ =>
            {
                for (int i = 0; i < Iterations; i++) HighLoadServer.HighLoadServer.GetCount();
            });
        }

        [Benchmark]
        public void HighLoadServer_ConcurrentWritesNoReads()
        {
            Parallel.For(0, ThreadCount, _ =>
            {
                for (int i = 0; i < Iterations; i++) HighLoadServer.HighLoadServer.AddToCount(1);
            });
        }

        [Benchmark]
        public void HighLoadServer_ReadAndWrite()
        {
            Parallel.Invoke(HighLoadServer_ConcurrentWritesNoReads, HighLoadServer_ConcurrentReadsNoWrites);
        }
    }
}
