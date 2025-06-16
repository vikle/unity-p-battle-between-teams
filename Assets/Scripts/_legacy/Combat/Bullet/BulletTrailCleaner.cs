using UnityEngine;

namespace Scorewarrior.Test.Views
{
    // Очищает последнюю позицию в TrailRenderer, чтобы не было бага когда берешь объект из пула.
    [RequireComponent(typeof(TrailRenderer))]
    public class BulletTrailCleaner : MonoBehaviour
    {
        TrailRenderer _trailRenderer;
        
        void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        void OnDisable()
        {
            _trailRenderer.Clear();
        }
    }
}
