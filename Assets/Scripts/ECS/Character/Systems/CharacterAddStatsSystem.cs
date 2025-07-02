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
    public sealed class CharacterAddStatsSystem : IEntityInitializeSystem
    {
        [Preserve]public CharacterAddStatsSystem(Pipeline pipeline) { }
        
        public void OnAfterEntityCreated(Pipeline pipeline, Entity entity)
        {
            if (!entity.HasComponent<CharacterMarker>())
            {
                return;
            }
            
            var marker = entity.GetComponent<CharacterMarker>();
            var modifiers = marker.modifiers;
            var stats = marker.stats;
            
            var descriptor = entity.GetComponent<ObjectRef<CharacterDescriptor>>().Target;
            
            stats.AddComponent<Accuracy>().value = (descriptor.Accuracy * modifiers.GetComponent<Accuracy>().value);
            stats.AddComponent<Dexterity>().value = (descriptor.Dexterity * modifiers.GetComponent<Dexterity>().value);
            stats.AddComponent<Health>().value = (descriptor.MaxHealth * modifiers.GetComponent<Health>().value);
            stats.AddComponent<Armor>().value = (descriptor.MaxArmor * modifiers.GetComponent<Armor>().value);
            stats.AddComponent<AimTime>().value = (descriptor.AimTime * modifiers.GetComponent<AimTime>().value);
        }
    };
}
