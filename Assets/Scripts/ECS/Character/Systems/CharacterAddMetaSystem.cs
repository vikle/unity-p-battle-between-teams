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
            
            var meta_entity = entity.GetComponent<CharacterMarker>().metaEntity;
            
            meta_entity.AddComponent<Team>();
            meta_entity.AddComponent<Sector>();
            meta_entity.AddComponent<CharacterState>();
            meta_entity.AddComponent<CharacterTarget>();
        }
    };
}
