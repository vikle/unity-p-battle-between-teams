using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class SpawnCharacterRequest : IPromise
    {
        public EPromiseState State { get; set; }
        
        public GameObject prefab;
        public Vector3 position;
        public ETeam team;
        public uint sector;
    };
}
