using System.Threading;
using Core.Services.Loaders;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Utils
{
    public class AssetsHelper : IAssetsLoader
    { 
        public async UniTask<T> LoadAssetWithCancellation<T>(string assetName, CancellationToken cancellationToken)
        {
            var matchConfig = await LoadAssetAsync<T>(assetName, cancellationToken);
            return matchConfig;
        }
        
        private static UniTask<T> LoadAssetAsync<T>(string address, CancellationToken cancellationToken)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            return handle.ToUniTask(cancellationToken: cancellationToken);
        } 
    }
}