using System;
using Core.Services.Events;
using Game.Events;
using UnityEngine;
using VContainer;

namespace Game.MonoBehaviourComponents
{
    public class RunnerControllerComponent : MonoBehaviour, IDisposable
    {
        [SerializeField] private float _jumpFactor;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Transform _transform;
        [SerializeField] private Animator _animator;

        private IDispatcherService _dispatcherService;

// TODO MOVE PLAYER NOT WORLD
        [Inject]
        public void Construct(IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
            _dispatcherService.Subscribe<PlayerJumpEvent>(OnPlayerJump);
            _transform = transform;
        }

        private void OnPlayerJump(PlayerJumpEvent obj)
        {
            _rigidbody.AddForce(new Vector2(0, _jumpFactor));
            _animator.SetTrigger(StringConstants.JumpTriggerName);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(StringConstants.OBSTACLE_LAYER))
            {
                _dispatcherService.Dispatch(new RoundOverEvent());
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer(StringConstants.GROUND_LAYER))
            {
                _animator.ResetTrigger(StringConstants.JumpTriggerName);
            }
        }

        public void Dispose()
        {
            _dispatcherService.Unsubscribe<PlayerJumpEvent>(OnPlayerJump);
        }

        public void Activate()
        {
            _transform.gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _transform.gameObject.SetActive(false);
        }

        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }
    }
}