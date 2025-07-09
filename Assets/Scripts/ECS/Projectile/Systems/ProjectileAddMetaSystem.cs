using UnityEngine;
using UnityEngine.Scripting;
using UniversalEntities;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Scorewarrior.ECS
{
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public sealed class ProjectileAddMetaSystem : IEntityInitializeSystem
    {
        [Preserve]public ProjectileAddMetaSystem(Pipeline pipeline) { }

        public void OnAfterEntityCreated(Pipeline pipeline, Entity entity)
        {
            if (!entity.HasComponent<ProjectileMarker>())
            {
                return;
            }

            var marker = entity.GetComponent<ProjectileMarker>();
            var meta = marker.meta;

            meta.AddComponent<Damage>().value = 0f;

            var target = meta.AddComponent<ProjectileTarget>();
            target.hit = false;
            target.entity = null;
            target.position = Vector3.zero;
            target.distance = 0f;
            
            var move_meta = meta.AddComponent<ProjectileMoveMeta>();
            move_meta.origin = Vector3.zero;
            move_meta.direction = Vector3.zero;
            move_meta.rayPosition = 0f;
        }
    };
}
