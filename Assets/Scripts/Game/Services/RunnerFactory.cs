using System.Threading;
using Core.Services.Factories;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.MonoBehaviourComponents.LoadingAssets;
using NUnit.Framework;
using VContainer;

namespace Game.Services
{
    public class RunnerFactory : AssetsFactoryBase
    {
        private RunnerControllerComponent _runnerInstance;
        private readonly IObjectResolver _resolver;
        
        public RunnerFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        
        public async UniTask Init(AssetsConfig assetsConfig, CancellationToken cancellationToken)
        {
            ValidateMatchConfigAsserts(assetsConfig);
            try
            {
                _runnerInstance = await LoadPrefab<RunnerControllerComponent>(assetsConfig.RunnerPrefab, cancellationToken);
                _resolver.Inject(_runnerInstance);
            }
            catch
            {
                Clear();
                throw;
            }
        }
        
        private void ValidateMatchConfigAsserts(AssetsConfig assetsConfig)
        {
            Assert.IsNotNull(assetsConfig, "AssetsConfig is null");
            Assert.IsNotNull(assetsConfig.RunnerPrefab, "Runner is null");
        }

        public RunnerControllerComponent GetRunnerInstance()
        {
           return _runnerInstance;
        }

        public override void Clear()
        {
            _runnerInstance.Dispose();
            base.Clear();
        }
    }
}