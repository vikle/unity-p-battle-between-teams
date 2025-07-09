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
            var meta = marker.meta;
            var stats = marker.stats;

            meta.AddComponent<Sector>().value = 0u;
            meta.AddComponent<Team>().value = ETeam.None;
            meta.AddComponent<CharacterTarget>().entity = null;
            meta.AddComponent<CharacterWeapon>().entity = null;
            meta.AddComponent<CharacterHitBox>().transform = null;
            meta.AddComponent<CharacterState>().value = ECharacterState.Idle;
            meta.AddComponent<Armor>().value = stats.GetComponent<Armor>().value;
            meta.AddComponent<Health>().value = stats.GetComponent<Health>().value;
            meta.AddComponent<AimTime>().value = 0f;
        }
    };
}
