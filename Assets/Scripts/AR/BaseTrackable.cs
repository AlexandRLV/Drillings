using System;
using UnityEngine;

namespace AR
{
    public class BaseTrackable<T> : MonoBehaviour
    {
        public event Action<T> OnTrackableFound;
        public event Action<T> OnTrackableUpdated;
        public event Action OnTrackableLost;

        protected void InvokeOnTrackableFound(T obj)
        {
            OnTrackableFound?.Invoke(obj);
        }

        protected void InvokeOnTrackableUpdated(T obj)
        {
            OnTrackableUpdated?.Invoke(obj);
        }

        protected void InvokeOnTrackableLost()
        {
            OnTrackableLost?.Invoke();
        }
    }
}