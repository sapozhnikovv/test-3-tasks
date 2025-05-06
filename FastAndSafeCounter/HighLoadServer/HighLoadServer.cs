using System.Runtime.CompilerServices;

namespace HighLoadServer
{
    /// <summary>
    /// High-performance thread-safe Lock-free counter. Add/Read counter via Interlocked in ring buffer, 
    /// Each element of ring is allocated to a separate CPU cache line. Supports up to RINGBUFFER_COUNT parallel writer threads.
    /// </summary>
    public class HighLoadServer
    {
        public static class Settings 
        {
            /// <summary>
            /// Count should be ^2 because used in fast binary operations. Default val is Environment.ProcessorCount * 2
            /// </summary>
            public static int RINGBUFFER_COUNT = Environment.ProcessorCount * 2;
        }

        private const int ALMOST_ALL_CPU_CACHE_LINE_SIZE_BYTES = 64;
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = ALMOST_ALL_CPU_CACHE_LINE_SIZE_BYTES)]
        private struct PaddedCounter
        {
            [System.Runtime.InteropServices.FieldOffset(0)]
            public int Value;
        }


        private static readonly PaddedCounter[] _ring = new PaddedCounter[Settings.RINGBUFFER_COUNT];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetIndexInRing() => Thread.CurrentThread.ManagedThreadId & (Settings.RINGBUFFER_COUNT - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddToCount(int value) => Interlocked.Add(ref _ring[GetIndexInRing()].Value, value);

        public static int GetCount()
        {
            var total = 0;//no need MemoryBarrier here, Volatile.Read has acquire-barier
            for (var i = 0; i < Settings.RINGBUFFER_COUNT; i++) total += Volatile.Read(ref _ring[i].Value);
            return total;
        }
    }
}
