using System.Collections.Generic;
using System.Threading;
using Core.Services.Factories.Pools;
using Core.Services.Loaders.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Services.Factories
{
    public class ItemsFactory : IItemsFactory
    {
        private ObjectPool<IItem> _objectPool;
        private readonly List<AsyncOperationHandle<GameObject>> _handles = new();

        public async UniTask Init(MatchConfig matchConfig, CancellationToken cancellationToken)
        {
            try
            {
                List<IItem> prefabs = await LoadPrefabs(matchConfig.ItemPrefabs, cancellationToken);
                _objectPool = new ObjectPool<IItem>(prefabs);
            }
            catch
            {
                Clear();
                throw;
            }
        }

        private async UniTask<List<IItem>> LoadPrefabs(AssetReference[] itemPrefabs, CancellationToken cancellationToken)
        {
            var prefabs = new List<IItem>();

            foreach (AssetReference prefab in itemPrefabs)
            {
                AsyncOperationHandle<GameObject> handle = prefab.InstantiateAsync();
                _handles.Add(handle);

                GameObject instance = await handle.WithCancellation(cancellationToken);
                instance.SetActive(false);
                prefabs.Add(instance.GetComponent<IItem>());
            }

            return prefabs;
        }

        public IItem Get()
        {
            IItem item = _objectPool.GetRandomNext();
            return item;
        }

        public void Return(IItem item)
        {
            if (item == null) return;

            // I prefer pools not to be responsible for disabling objects
            item.Deactivate();
            _objectPool.ReturnToPool(item);
        }

        public void Clear()
        {
            foreach (AsyncOperationHandle<GameObject> handle in _handles)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }

            _handles.Clear();

            _objectPool?.Clear();

            _objectPool = null;
        }
    }
}