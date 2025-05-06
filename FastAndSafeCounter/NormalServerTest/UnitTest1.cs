namespace NormalServerTest
{
    public class UnitTest1
    {
        [Fact]
        public void TestConcurrency()
        {
            const int threadCount = 64;
            const int iterations = 1000000;
            var expected = threadCount * iterations;
            Parallel.For(0, threadCount, indx =>
            {
                var simulateRead = NormalServer.NormalServer.GetCount();
                for (int i = 0; i < iterations; i++)
                {
                    NormalServer.NormalServer.AddToCount(1);
                    if (i % 10 == indx)
                    {
                        simulateRead = NormalServer.NormalServer.GetCount();
                    }
                }
            });
            var actual = NormalServer.NormalServer.GetCount();
            Assert.Equal(expected, actual);
        }
    }
}