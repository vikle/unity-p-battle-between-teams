using UnityEngine.Scripting;
using UniversalEntities;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Scorewarrior.ECS
{
    using Test.Descriptors;
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public sealed class WeaponAddStatsSystem : IEntityInitializeSystem
    {
        [Preserve]public WeaponAddStatsSystem(Pipeline pipeline) { }

        public void OnAfterEntityCreated(Pipeline pipeline, Entity entity)
        {
            if (!entity.HasComponent<WeaponMarker>())
            {
                return;
            }
            
            var marker = entity.GetComponent<WeaponMarker>();
            var modifiers_entity = marker.modifiers;
            var stats_entity = marker.stats;
            
            var descriptor = entity.GetComponent<ObjectRef<WeaponDescriptor>>().Target;

            stats_entity.AddComponent<Damage>().value = (descriptor.Damage * modifiers_entity.GetComponent<Damage>().value);
            stats_entity.AddComponent<Accuracy>().value = (descriptor.Accuracy * modifiers_entity.GetComponent<Accuracy>().value);
            stats_entity.AddComponent<FireRate>().value = (descriptor.FireRate * modifiers_entity.GetComponent<FireRate>().value);
            stats_entity.AddComponent<ClipSize>().value = (uint)(descriptor.ClipSize * (modifiers_entity.GetComponent<ClipSize>().value / 100f));
            stats_entity.AddComponent<ReloadTime>().value = (descriptor.ReloadTime * modifiers_entity.GetComponent<ReloadTime>().value);
        }
    };
}
