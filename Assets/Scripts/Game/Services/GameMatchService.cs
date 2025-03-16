using System.Threading;
using Core.Services.Events;
using Core.Services.Loaders;
using Core.Services.Match;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.MonoBehaviourComponents;
using VContainer;
using VContainer.Unity;

namespace Game.Services
{
    public class GameMatchService : MatchServiceBase, ITickable
    {
        private readonly IRoundLogic _roundLogic;
        private readonly ObstaclesFactory _obstaclesFactory;
        private readonly ColumnsFactory _columnsFactory;
        private readonly RunnerFactory _runnerFactory;
        private readonly LevelManagerComponent _levelManager;

        public GameMatchService(LevelManagerComponent levelManager, IObjectResolver objectResolver,
            IDispatcherService dispatcherService) : base(dispatcherService)
        {
            _levelManager = levelManager;
            _obstaclesFactory = new ObstaclesFactory();
            _columnsFactory = new ColumnsFactory();
            _runnerFactory = new RunnerFactory(objectResolver);
            _roundLogic = new RoundLogic(dispatcherService, _obstaclesFactory);
        }

        // here could be added percentage update events for Loading screen, after each await
        public override async UniTask BuildScene(IAssetsLoader assetsLoader, CancellationToken buildCancellationToken)
        {
            await base.BuildScene(assetsLoader, buildCancellationToken);
            var matchConfig =
                await assetsLoader.LoadAssetWithCancellation<MatchConfig>(StringConstants.MATCH_CONFIG_ADDRESS,
                    buildCancellationToken);
            await _obstaclesFactory.Init(matchConfig, buildCancellationToken);
            await _columnsFactory.Init(matchConfig, buildCancellationToken);
            await _runnerFactory.Init(matchConfig, buildCancellationToken);
        }

        protected override void StartRoundLogic()
        {
            _levelManager.InitEvents();
            _levelManager.InitColumns(_columnsFactory.GetAllColumns());
            _levelManager.InitObstacles();
            _levelManager.InitRunner(_runnerFactory.GetRunnerInstance());
            
            _roundLogic.StartRound(RoundTokenSource.Token);
        }

        public void Tick()
        {
            _roundLogic.OnTick();
        }

        protected override void DisposeRound()
        {
            _roundLogic.EndRound();
            _obstaclesFactory.Clear();
            _columnsFactory.Clear();
            _runnerFactory.Clear();
            _levelManager.Dispose();
        }
    }
}