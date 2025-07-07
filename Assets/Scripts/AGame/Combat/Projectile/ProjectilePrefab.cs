using UnityEngine;

namespace Scorewarrior
{
    public sealed class ProjectilePrefab : MonoBehaviour
    {
        [Min(1f)]public float moveSpeed = 30f;
    };
}