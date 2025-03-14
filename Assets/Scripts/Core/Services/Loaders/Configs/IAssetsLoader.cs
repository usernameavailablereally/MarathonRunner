using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Services.Loaders.Configs
{
    public interface IAssetsLoader
    {
        UniTask<MatchConfig> LoadAndValidateMatchConfig(CancellationToken cancellationToken);
    }
}