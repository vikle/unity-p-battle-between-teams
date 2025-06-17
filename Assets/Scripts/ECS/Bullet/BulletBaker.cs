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
        }
        
        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            
        }
    };
}
