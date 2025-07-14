using UnityEngine.Scripting;
using UniversalEntities;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Scorewarrior.ECS
{
    using Descriptors;
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public sealed class CharacterAnimationAddMetaSystem : IEntityInitializeSystem
    {
        [Preserve]public CharacterAnimationAddMetaSystem(Pipeline pipeline) { }

        public void OnAfterEntityCreated(Pipeline pipeline, Entity entity)
        {
            if (!entity.HasComponent<CharacterAnimationMarker>())
            {
                return;
            }

            var meta = entity.GetComponent<CharacterAnimationMarker>().meta;
            
            var hash = meta.AddComponent<CharacterAnimationHash>();
            var validation = meta.AddComponent<CharacterAnimationHashValidation>();
            var descriptor = entity.GetComponent<ObjectRef<CharacterAnimationDescriptor>>().Target;
            
            AnimHash.Get(descriptor._aimingName, out hash.aiming, out validation.aiming);
            AnimHash.Get(descriptor._reloadingName, out hash.reloading, out validation.reloading);
            AnimHash.Get(descriptor._shootName, out hash.shoot, out validation.shoot);
            AnimHash.Get(descriptor._reloadTimeName, out hash.reloadTime, out validation.reloadTime);
            AnimHash.Get(descriptor._dieName, out hash.die, out validation.die);
            
            meta.AddComponent<ReloadTime>().value = descriptor._reloadAnimationLength;
            
            pipeline.ForceUpdateFilters(meta);
        }
    };
}
