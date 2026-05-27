using System;
using System.Collections.Generic;
using UnityEngine;

namespace JamalArouna.Library.Systems
{
    [Serializable]
    public class LockSystem<TLock> where TLock : struct, Enum
    {
        [SerializeField] private readonly Dictionary<TLock, int> lockCounts = new();

        public event Action<TLock> OnLocked;
        public event Action<TLock> OnUnlocked;
        public event Action OnAnyLockChanged;

        public bool IsLocked(TLock lockType) =>
            lockCounts.TryGetValue(lockType, out int count) && count > 0;

        public bool IsAnyLocked => lockCounts.Count > 0;

        public IDisposable Lock(TLock lockType)
        {
            bool wasLocked = IsLocked(lockType);

            lockCounts.TryGetValue(lockType, out int count);
            lockCounts[lockType] = count + 1;

            if (!wasLocked)
            {
                OnLocked?.Invoke(lockType);
                OnAnyLockChanged?.Invoke();
            }

            return new LockHandle(this, lockType);
        }

        public IDisposable LockAll()
        {
            List<IDisposable> handles = new();

            foreach (TLock lockType in Enum.GetValues(typeof(TLock)))
                handles.Add(Lock(lockType));

            return new MultiLockHandle(handles);
        }

        public IDisposable LockMany(params TLock[] lockTypes)
        {
            List<IDisposable> handles = new();

            foreach (TLock lockType in lockTypes)
                handles.Add(Lock(lockType));

            return new MultiLockHandle(handles);
        }

        public void UnlockAll()
        {
            if (lockCounts.Count == 0)
                return;

            List<TLock> lockedTypes = new(lockCounts.Keys);
            lockCounts.Clear();

            foreach (TLock lockType in lockedTypes)
                OnUnlocked?.Invoke(lockType);

            OnAnyLockChanged?.Invoke();
        }

        private void Unlock(TLock lockType)
        {
            if (!lockCounts.TryGetValue(lockType, out int count))
                return;

            count--;

            if (count <= 0)
            {
                lockCounts.Remove(lockType);
                OnUnlocked?.Invoke(lockType);
                OnAnyLockChanged?.Invoke();
            }
            else
            {
                lockCounts[lockType] = count;
            }
        }

        private sealed class LockHandle : IDisposable
        {
            private readonly LockSystem<TLock> system;
            private readonly TLock lockType;
            private bool disposed;

            public LockHandle(LockSystem<TLock> system, TLock lockType)
            {
                this.system = system;
                this.lockType = lockType;
            }

            public void Dispose()
            {
                if (disposed)
                    return;

                system.Unlock(lockType);
                disposed = true;
            }
        }

        private sealed class MultiLockHandle : IDisposable
        {
            private readonly List<IDisposable> handles;
            private bool disposed;

            public MultiLockHandle(List<IDisposable> handles) => this.handles = handles;

            public void Dispose()
            {
                if (disposed)
                    return;

                foreach (IDisposable handle in handles)
                    handle.Dispose();

                disposed = true;
            }
        }
    }
}