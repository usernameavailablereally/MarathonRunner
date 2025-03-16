using System;
using System.Collections.Concurrent;

namespace Core.Services.Events
{
    public class DispatcherService : IDispatcherService, IDisposable
    {
        /// Thread-safe dictionary
        private readonly ConcurrentDictionary<Type, Delegate> _eventListeners = new();
        private readonly ConcurrentDictionary<Type, byte> _processingEvents = new();

        public void Subscribe<T>(Action<T> listener) where T : GameEventBase
        {
            _eventListeners.AddOrUpdate(
                typeof(T),
                listener,
                (_, existing) => Delegate.Combine(existing, listener));
        }

        public void Unsubscribe<T>(Action<T> listener) where T : GameEventBase
        {
            if (_eventListeners.TryGetValue(typeof(T), out Delegate existing))
            {
                Delegate current = Delegate.Remove(existing, listener);
                if (current == null)
                {
                    _eventListeners.TryRemove(typeof(T), out _);
                }
                else
                {
                    _eventListeners[typeof(T)] = current;
                }
            }
        }

        public void Dispatch(GameEventBase eventData)
        {
            Type eventType = eventData.GetType();
            if (!_processingEvents.TryAdd(eventType, 0)) return;

            try
            {
                if (_eventListeners.TryGetValue(eventType, out Delegate handler) && handler != null)
                {
                    foreach (Delegate d in handler.GetInvocationList())
                    {
                        try
                        {
                            d.DynamicInvoke(eventData);
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    }
                }
            }
            finally
            {
                _processingEvents.TryRemove(eventType, out _);
            }
        }

        public void ClearAllSubscriptions()
        {
            _eventListeners.Clear();
        }

        public void Dispose()
        {
            _eventListeners.Clear();
            _processingEvents.Clear();
            GC.SuppressFinalize(this);
        }
    }
}