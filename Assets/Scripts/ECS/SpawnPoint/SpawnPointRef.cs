using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class SpawnPointRef : IComponent
    {
        public SpawnPoint originalObject;
        public Transform transform;
    };
}
