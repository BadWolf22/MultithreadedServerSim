namespace MultithreadedServerSim.Helpers;

internal static class LockHelper
{
    #region Monitor.TryEnter
    // An example that will continuously try to enter the monitor. Waiting has breaks so you could return an update in the meantime.

    public static TReturn ExecuteWithTryMonitor<TReturn>(Func<TReturn> action, Object lockObject)
    {
        while (true)
        {
            if (!Monitor.TryEnter(lockObject, 200))
                continue;
            try
            {
                return action.Invoke();
            }
            finally
            {
                Monitor.Exit(lockObject);
            }
        }
    }

    public static void ExecuteWithTryMonitor(Action action, Object lockObject)
    {
        ExecuteWithTryMonitor(() => { action.Invoke(); return true; }, lockObject);
    }

    #endregion

    #region Monitor.Enter
    // Longform of lock syntax

    public static TReturn ExecuteWithMonitor<TReturn>(Func<TReturn> action, Object lockObject)
    {
        Monitor.Enter(lockObject);
        try
        {
            return action.Invoke();
        }
        finally
        {
            Monitor.Exit(lockObject);
        }
    }

    public static void ExecuteWithMonitor(Action action, Object lockObject)
    {
        ExecuteWithMonitor(() => { action.Invoke(); return true; }, lockObject);
    }

    #endregion

    #region Lock
    // Simple, shorthand for Monitor.Enter

    public static TReturn ExecuteWithLock<TReturn>(Func<TReturn> action, Object lockObject)
    {
        lock (lockObject)
        {
            return action.Invoke();
        }
    }

    public static void ExecuteWithLock(Action action, Object lockObject)
    {
        ExecuteWithLock(() => { action.Invoke(); return true; }, lockObject);
    }

    #endregion

    #region Mutex
    // The benefit of Mutex is that it can be used across multiple processes (if given a name) instead of just shared memory (at the cost of a bit of performance).

    public static TReturn ExecuteWithMutex<TReturn>(Func<TReturn> action, string name)
    {
        using var mutex = new Mutex(false, name);
        mutex.WaitOne();
        try
        {
            return action.Invoke();
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    public static void ExecuteWithMutex(Action action, string name)
    {
        ExecuteWithMutex(() => { action.Invoke(); return true; }, name);
    }

    #endregion Mutex

    #region ReadWriteLock

    public static TReturn ExecuteWithReadLock<TReturn>(Func<TReturn> action, ReaderWriterLockSlim lockObject)
    {
        var acquired = false;
        try
        {
            lockObject.EnterReadLock();
            acquired = true;
            return action.Invoke();
        }
        finally
        {
            if (acquired)
            {
                lockObject.ExitReadLock();
            }
        }
    }

    public static void ExecuteWithWriteLock(Action action, ReaderWriterLockSlim lockObject)
    {
        var acquired = false;
        try
        {
            lockObject.EnterWriteLock();
            acquired = true;
            action.Invoke();
        }
        finally
        {
            if (acquired)
            {
                lockObject.ExitWriteLock();
            }
        }
    }

    #endregion
}
