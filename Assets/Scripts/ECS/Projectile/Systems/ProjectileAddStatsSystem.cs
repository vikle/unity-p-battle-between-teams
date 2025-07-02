using UnityEngine.Scripting;
using UniversalEntities;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Scorewarrior.ECS
{
    using Test.Views;
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public sealed class ProjectileAddStatsSystem : IEntityInitializeSystem
    {
        [Preserve]public ProjectileAddStatsSystem(Pipeline pipeline) { }

        public void OnAfterEntityCreated(Pipeline pipeline, Entity entity)
        {
            if (!entity.HasComponent<ProjectileMarker>())
            {
                return;
            }

            var marker = entity.GetComponent<ProjectileMarker>();
            var stats = marker.stats;
            
            var prefab = entity.GetComponent<ObjectRef<ProjectilePrefab>>().Target;
            
            stats.AddComponent<Speed>().value = prefab.moveSpeed;
        }
    };
}
