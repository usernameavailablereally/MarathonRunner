using System.Threading;
using Core.Services.Factories;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.MonoBehaviourComponents;
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
        
        public async UniTask Init(MatchConfig matchConfig, CancellationToken cancellationToken)
        {
            ValidateMatchConfigAsserts(matchConfig);
            try
            {
                _runnerInstance = await LoadPrefab<RunnerControllerComponent>(matchConfig.RunnerPrefab, cancellationToken);
                _resolver.Inject(_runnerInstance);
            }
            catch
            {
                Clear();
                throw;
            }
        }
        
        private void ValidateMatchConfigAsserts(MatchConfig matchConfig)
        {
            Assert.IsNotNull(matchConfig, "MatchConfig is null");
            Assert.IsNotNull(matchConfig.RunnerPrefab, "Runner is null");
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