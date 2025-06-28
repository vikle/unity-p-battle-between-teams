using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    using Test.Views;
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BulletPrefab))]
    public sealed class BulletBaker : EntityActorBaker
    {
        public BulletPrefab prefab;
        
#if UNITY_EDITOR
        void OnValidate()
        {
            if (prefab == null) prefab = GetComponent<BulletPrefab>();
        }
#endif
        public override void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            entity.AddComponent<BulletMarker>();
            entity.AddComponent<ObjectRef<BulletPrefab>>().Target = prefab;
        }
        
        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor) { }
    };
}
