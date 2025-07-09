using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class ProjectileSpawnCommand : IEvent
    {
        public GameObject prefab;
        public Vector3 position;
        public Entity target;
        public float damage;
        public bool hit;
        public Vector3 hitBoxPosition;
    };
}
