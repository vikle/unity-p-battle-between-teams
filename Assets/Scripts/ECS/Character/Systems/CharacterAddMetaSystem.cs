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
    public sealed class CharacterAddMetaSystem : IEntityInitializeSystem
    {
        [Preserve]public CharacterAddMetaSystem(Pipeline pipeline) { }
        
        public void OnAfterEntityCreated(Pipeline pipeline, Entity entity)
        {
            if (!entity.HasComponent<CharacterMarker>())
            {
                return;
            }
            
            var marker = entity.GetComponent<CharacterMarker>();
            var meta_entity = marker.metaEntity;
            
            meta_entity.AddComponent<Team>();
            meta_entity.AddComponent<Sector>();
            meta_entity.AddComponent<CharacterState>().value = ECharacterState.Idle;
            meta_entity.AddComponent<CharacterTarget>().value = null;
            meta_entity.AddComponent<AimTime>().value = 0f;
            
            ref float health_value = ref marker.statsEntity.GetComponent<Health>().value;
            health_value *= marker.modifiersEntity.GetComponent<Health>().value;
            meta_entity.AddComponent<Health>().value = health_value;
            
            ref float armor_value = ref marker.statsEntity.GetComponent<Armor>().value;
            armor_value *= marker.modifiersEntity.GetComponent<Armor>().value;
            meta_entity.AddComponent<Armor>().value = armor_value;
            
            
            
        }
    };
}
