using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Services.Factories
{
    public abstract class AssetsFactoryBase 
    {
        private readonly List<AsyncOperationHandle<GameObject>> _handles = new();
        
        protected async UniTask<T> LoadPrefab<T>(AssetReference prefabReference, CancellationToken cancellationToken)  
        { 
            GameObject instance = await LoadAssetReference(cancellationToken, prefabReference); 
            return instance.GetComponent<T>();
        }

        protected async UniTask<List<T>> LoadPrefabs<T>(AssetReference[] prefabReferences, CancellationToken cancellationToken)
        {
            var prefabs = new List<T>();

            foreach (AssetReference prefab in prefabReferences)
            {
                GameObject instance = await LoadAssetReference(cancellationToken, prefab); 
                prefabs.Add(instance.GetComponent<T>());
            }

            return prefabs;
        }

        //TODO: RUn on dispose (check)
        public virtual void Clear()
        {
            foreach (AsyncOperationHandle<GameObject> handle in _handles)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }

            _handles.Clear();
        }

        private async Task<GameObject> LoadAssetReference(CancellationToken cancellationToken, AssetReference prefab)
        {
            AsyncOperationHandle<GameObject> handle = prefab.InstantiateAsync();
            _handles.Add(handle);

            GameObject instance = await handle.WithCancellation(cancellationToken);
            instance.SetActive(false);
            return instance;
        }
    }
    
}