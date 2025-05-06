namespace HighLoadServerTest
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
                var simulateRead = HighLoadServer.HighLoadServer.GetCount();
                for (int i = 0; i < iterations; i++)
                {
                    HighLoadServer.HighLoadServer.AddToCount(1);
                    if (i % 10 == indx)
                    {
                        simulateRead = HighLoadServer.HighLoadServer.GetCount();
                    }
                }
            });
            var actual = HighLoadServer.HighLoadServer.GetCount();
            Assert.Equal(expected, actual);
        }
    }
}