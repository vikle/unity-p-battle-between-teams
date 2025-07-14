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
            meta.AddComponent<ProjectileTarget>();
            meta.AddComponent<ProjectileMoveMeta>();
            
            pipeline.ForceUpdateFilters(meta);
        }
    };
}
