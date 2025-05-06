namespace NormalServer
{
    public class NormalServer
    {
        private static int count;
        private static readonly ReaderWriterLockSlim rwl = new();

        public static int GetCount()
        {
            rwl.EnterReadLock();
            try
            {
                return count;
            }
            finally
            {
                rwl.ExitReadLock();
            }
        }

        public static void AddToCount(int value)
        {
            rwl.EnterWriteLock();
            try
            {
                count += value;
            }
            finally
            {
                rwl.ExitWriteLock();
            }
        }
    }
}
