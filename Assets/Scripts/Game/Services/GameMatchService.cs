using System.Threading;
using Core.Services.Events;
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
        private readonly MatchConfig _matchConfig;
        private readonly AssetsConfig _assetsConfig;

        public GameMatchService(
            MatchConfig matchConfig, 
            AssetsConfig assetsConfig,
            LevelManagerComponent levelManager,
            IObjectResolver objectResolver,
            IDispatcherService dispatcherService) : base(dispatcherService)
        {
            _matchConfig = matchConfig;
            _assetsConfig = assetsConfig;
            _levelManager = levelManager;
            _obstaclesFactory = new ObstaclesFactory();
            _columnsFactory = new ColumnsFactory();
            _runnerFactory = new RunnerFactory(objectResolver);
            _roundLogic = new RoundLogic(_matchConfig, dispatcherService, _obstaclesFactory);
        }

        // here could be added percentage update events for Loading screen, after each await
        public override async UniTask BuildScene(CancellationToken buildCancellationToken)
        {
            await base.BuildScene(buildCancellationToken);
            await _obstaclesFactory.Init(_assetsConfig, buildCancellationToken);
            await _columnsFactory.Init(_assetsConfig, buildCancellationToken);
            await _runnerFactory.Init(_assetsConfig, buildCancellationToken);
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