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
    public sealed class WeaponAddMetaSystem : IEntityInitializeSystem
    {
        [Preserve]public WeaponAddMetaSystem(Pipeline pipeline) { }

        public void OnAfterEntityCreated(Pipeline pipeline, Entity entity)
        {
            if (!entity.HasComponent<WeaponMarker>())
            {
                return;
            }

            var marker = entity.GetComponent<WeaponMarker>();

            marker.meta.AddComponent<FireRate>()
                  .value = 0f;
            
            marker.meta.AddComponent<ClipSize>()
                  .value = marker.stats.GetComponent<ClipSize>().value;
        }
    };
}
