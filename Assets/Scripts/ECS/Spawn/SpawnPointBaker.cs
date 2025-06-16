using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    [RequireComponent(typeof(SpawnPoint))]
    [DisallowMultipleComponent]
    public sealed class SpawnPointBaker : EntityActorBaker
    {
        public override void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var sp_ref = entity.AddComponent<SpawnPointRef>();
            sp_ref.originalObject = GetComponent<SpawnPoint>();
            sp_ref.transform = transform;
        }
        
        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor) { }
    };
}
