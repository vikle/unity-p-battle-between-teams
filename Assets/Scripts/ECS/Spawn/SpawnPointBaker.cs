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
            var obj_ref = entity.AddComponent<SpawnPointRef>();
            obj_ref.originalObject = GetComponent<SpawnPoint>();
            obj_ref.transform = transform;
        }
        
        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor) { }
    };
}
