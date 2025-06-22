using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    using Test.Views;
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BulletPrefab))]
    public sealed class BulletBaker : EntityActorBaker
    {
        public override void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            entity.AddComponent<BulletMarker>();
            
            var obj_ref = entity.AddComponent<BulletPrefabRef>();
            obj_ref.originalObject = GetComponent<BulletPrefab>();
            obj_ref.transform = transform;
        }
        
        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor) { }
    };
}
