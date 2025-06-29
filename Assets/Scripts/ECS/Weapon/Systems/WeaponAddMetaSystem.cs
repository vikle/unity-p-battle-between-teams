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

            var meta_entity = entity.GetComponent<WeaponMarker>().metaEntity;
            
            
        }
    };
}
