using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Services.Loaders
{
    public interface IAssetsLoader
    {
        UniTask<T> LoadAssetWithCancellation<T>(string assetName, CancellationToken cancellationToken);
    }
}