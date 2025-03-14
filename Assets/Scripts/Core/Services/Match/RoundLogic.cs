using System.Threading;
using Core.Services.Events;
using Core.Services.Factories;

namespace Core.Services.Match
{
    public class RoundLogic 
    {
        private readonly IDispatcherService _dispatcherService;
        // Needed for linking round-related Tasks (e.g. animations, delays)
        private CancellationToken _roundCancellationToken;
        public RoundLogic(IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
        }
        
        public void StartRound(IItem currentItems, CancellationToken roundCancellationToken)
        {
            _roundCancellationToken = roundCancellationToken;
            _dispatcherService.Subscribe<ClickableClickedEvent>(OnClickableClicked);
        }

        private void OnClickableClicked(ClickableClickedEvent unknownItem)
        {
        }

        public void EndRound()
        {
            _dispatcherService.Unsubscribe<ClickableClickedEvent>(OnClickableClicked);
        }
    }
}