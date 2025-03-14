using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Services.Loaders.Configs
{
    public class AssetsHelper : IAssetsLoader
    { 
        public async UniTask<MatchConfig> LoadAndValidateMatchConfig(CancellationToken cancellationToken)
        {
            var matchConfig = await LoadAssetAsync<MatchConfig>(StringConstants.MATCH_CONFIG_ADDRESS, cancellationToken);
            ValidateMatchConfigAsserts(matchConfig);
            return matchConfig;
        }
        
        private static UniTask<T> LoadAssetAsync<T>(string address, CancellationToken cancellationToken)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            return handle.ToUniTask(cancellationToken: cancellationToken);
        }

        private void ValidateMatchConfigAsserts(MatchConfig config)
        {
            Assert.IsNotNull(config, "MatchConfig is null");
            Assert.IsTrue(config.ItemPrefabs.Length > 0, "ItemPrefabs array is empty");
        }
    }
}