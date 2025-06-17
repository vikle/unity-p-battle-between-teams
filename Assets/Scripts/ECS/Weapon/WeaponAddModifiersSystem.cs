using UnityEngine;
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
    public sealed class WeaponAddModifiersSystem : IEntityInitializeSystem
    {
        [Preserve]public WeaponAddModifiersSystem(Pipeline pipeline) { }
        
        public void OnAfterEntityCreated(Pipeline pipeline, Entity entity)
        {
            var descriptor = entity.GetComponent<ObjectRef<GameObject>>()
                                   .Target.GetComponent<WeaponDescriptor>();

            var stats_entity = entity.GetComponent<CharacterMarker>().statsEntity;
            
            stats_entity.AddComponent<Damage>().value = descriptor.Damage;
            stats_entity.AddComponent<Accuracy>().value = descriptor.Accuracy;
            stats_entity.AddComponent<FireRate>().value = descriptor.FireRate;
            stats_entity.AddComponent<ClipSize>().value = descriptor.ClipSize;
            stats_entity.AddComponent<ReloadTime>().value = descriptor.ReloadTime;
        }
    };
}
