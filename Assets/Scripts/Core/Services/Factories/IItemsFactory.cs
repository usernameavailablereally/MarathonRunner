using System.Threading;
using Core.Services.Loaders.Configs;
using Cysharp.Threading.Tasks;

namespace Core.Services.Factories
{
    public interface IItemsFactory
    {
        UniTask Init(MatchConfig matchConfig, CancellationToken cancellationToken);
        IItem Get();
        void Return(IItem item);
        void Clear();
    }
}