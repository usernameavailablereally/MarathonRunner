using System;
using System.Collections.Generic;

namespace Core.Services.Factories.Pools
{
    // Manager for scene components. It does not have all the addressables handles
    // Managing Addressables resources happens in FactoryBase scripts
    public class ObjectPool<T>
    {
        private readonly List<T> _objects;
        private readonly Random _random = new();
        public ObjectPool(List<T> instantiatedPrefabs)
        {
            _objects = instantiatedPrefabs;
        }

        public T GetRandomNext()
        {
            if (_objects.Count <= 0)
            {
                // No overflow support needed, since the pool size is fixed
                throw new Exception("Error in getting item. Pool does not support overflow.");
            }
            
            int index = _random.Next(_objects.Count);
            T obj = _objects[index];
            _objects.RemoveAt(index);
            return obj;
        }

        public void ReturnToPool(T obj)
        {
            _objects.Add(obj);
        }
        
        // this only disposes pool, not the Addressables handles
        public void Dispose()
        {
            _objects.Clear();
        }
    }
}