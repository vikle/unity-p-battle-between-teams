using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class CharacterSpawnTask : IEvent
    {
        public GameObject prefab;
        public Vector3 position;
        public ETeam team;
        public uint sector;
    };
}
