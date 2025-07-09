using UnityEngine;

namespace Scorewarrior.Test.Views
{
    [RequireComponent(typeof(TrailRenderer))]
    public sealed class BulletTrailCleaner : MonoBehaviour
    {
        TrailRenderer m_trailRenderer;

        void Awake()
        {
            m_trailRenderer = GetComponent<TrailRenderer>();
        }

        void OnDisable()
        {
            m_trailRenderer.Clear();
        }
    };
}
