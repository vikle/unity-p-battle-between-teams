using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ProjectilePrefab))]
    public sealed class ProjectileBaker : EntityActorBaker
    {
        public ProjectilePrefab prefab;
        
#if UNITY_EDITOR
        void OnValidate()
        {
            if (prefab == null) prefab = GetComponent<ProjectilePrefab>();
        }
#endif
        public override void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.AddComponent<ProjectileMarker>();
            marker.stats = pipeline.CreateEntity();
            marker.meta = pipeline.CreateEntity();
            
            entity.AddComponent<ObjectRef<ProjectilePrefab>>().Target = prefab;
        }

        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.GetComponent<ProjectileMarker>();
            marker.stats.Destroy();
            marker.meta.Destroy();
        }
    };
}
