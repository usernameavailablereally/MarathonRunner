using System.Threading;
using Core.Services.Loaders;
using Cysharp.Threading.Tasks;

namespace Core.Services.Match
{
    public interface IMatchService
    {
        UniTask BuildScene(IAssetsLoader assetsLoader, CancellationToken buildCancellationToken);
        UniTask RunGame();
        void Dispose();
    }
}