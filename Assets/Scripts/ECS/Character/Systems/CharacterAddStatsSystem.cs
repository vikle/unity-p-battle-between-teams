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
            
            var stats_entity = entity.GetComponent<CharacterMarker>().statsEntity;
            var descriptor = entity.GetComponent<ObjectRef<CharacterDescriptor>>().Target;
            
            stats_entity.AddComponent<Accuracy>().value = descriptor.Accuracy;
            stats_entity.AddComponent<Dexterity>().value = descriptor.Dexterity;
            stats_entity.AddComponent<Health>().value = descriptor.MaxHealth;
            stats_entity.AddComponent<Armor>().value = descriptor.MaxArmor;
            stats_entity.AddComponent<AimTime>().value = descriptor.AimTime;
        }
    };
}
