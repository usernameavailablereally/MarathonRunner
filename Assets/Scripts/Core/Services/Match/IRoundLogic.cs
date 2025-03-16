using System.Threading;

namespace Core.Services.Match
{
    public interface IRoundLogic
    {
        void StartRound(CancellationToken roundCancellationToken);
        void EndRound();
        void OnTick();
    }
}