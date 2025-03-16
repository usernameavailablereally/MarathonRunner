using System;
using Core.Services.Events;
using Game.Events;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Game.MonoBehaviourComponents
{
   public class GameOverLayerComponent : MonoBehaviour, IDisposable
   {
      [SerializeField] private Button _restartButton;

      private IDispatcherService _dispatcherService;

      [Inject]
      public void Construct(IDispatcherService dispatcherService)
      {
         _dispatcherService = dispatcherService;
         _restartButton.onClick.AddListener(OnRestartButtonClick);
         _dispatcherService.Subscribe<RoundStartEvent>(OnRoundStart);
         _dispatcherService.Subscribe<RoundOverEvent>(OnRoundOver);
      }

      private void OnRestartButtonClick()
      {
         gameObject.SetActive(false);
         _dispatcherService.Dispatch(new RestartEntryPointEvent());
      }

      private void OnRoundStart(RoundStartEvent obj)
      {
         gameObject.SetActive(false);
      }

      private void OnRoundOver(RoundOverEvent data)
      {
         gameObject.SetActive(true);
      }

      public void Dispose()
      {
         _restartButton.onClick.RemoveAllListeners();
         _dispatcherService.Unsubscribe<RoundStartEvent>(OnRoundStart);
         _dispatcherService.Unsubscribe<RoundOverEvent>(OnRoundOver);
      }
   }
}
