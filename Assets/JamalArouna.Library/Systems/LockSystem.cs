using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameData.Scripts.Shared.Utillities
{
    [Serializable]
    public class LockSystem<TLock> where TLock : struct, Enum
    {
        [SerializeField] private readonly Dictionary<TLock, int> lockCounts = new();
    
        public bool IsLocked(TLock lockType) => lockCounts.TryGetValue(lockType, out int count) && count > 0;
    
        public IDisposable Lock(TLock lockType)
        {
            lockCounts.TryGetValue(lockType, out int count);
            lockCounts[lockType] = count + 1;

            return new LockHandle(this, lockType);
        }
    
        private void Unlock(TLock lockType)
        {
            if (!lockCounts.TryGetValue(lockType, out int count)) return;

            count--;

            if (count <= 0)
                lockCounts.Remove(lockType);
            else
                lockCounts[lockType] = count;
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
                if (disposed) return;

                system.Unlock(lockType);
                disposed = true;
            }
        }
    
        //Lock Multiple (IDisposable)
        public IDisposable LockMany(params TLock[] lockTypes)
        {
            List<IDisposable> handles = new();

            foreach (TLock lockType in lockTypes)
                handles.Add(Lock(lockType));

            return new MultiLockHandle(handles);
        }
    
        private sealed class MultiLockHandle : IDisposable
        {
            private readonly List<IDisposable> handles;
            private bool disposed;

            public MultiLockHandle(List<IDisposable> handles)
            {
                this.handles = handles;
            }

            public void Dispose()
            {
                if (disposed) return;

                foreach (IDisposable handle in handles)
                    handle.Dispose();

                disposed = true;
            }
        }
    
        public bool IsAnyLocked => lockCounts.Count > 0;
        public void UnlockAll() => lockCounts.Clear();
    }
}
