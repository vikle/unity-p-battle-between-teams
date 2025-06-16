using UnityEngine;

namespace Scorewarrior.Test.Services
{
    [DisallowMultipleComponent]
    public abstract class CachedMonoBehaviour : MonoBehaviour, IUpdateHandler
    {
        protected virtual void OnEnable()
        {
            UpdateEngine.RegisterHandler(this);
        }

        protected virtual void OnDisable()
        {
            UpdateEngine.UnregisterHandler(this);
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }
    }
}
