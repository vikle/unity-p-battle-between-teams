using System;
using System.Linq;
using UnityEngine.Scripting;
using UniversalEntities;
using Random = UnityEngine.Random;

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
            if (!entity.HasComponent<WeaponMarker>())
            {
                return;
            }
            
            var modifiers = entity.GetComponent<WeaponMarker>().modifiers;
            var provider = entity.GetComponent<ObjectRef<WeaponDescriptorProvider>>().Target;

            modifiers.AddComponent<Damage>().value = 1f;
            modifiers.AddComponent<Accuracy>().value = 1f;
            modifiers.AddComponent<FireRate>().value = 1f;
            modifiers.AddComponent<ClipSize>().value = 100u;
            modifiers.AddComponent<ReloadTime>().value = 1f;

            var available_modifiers = Enum.GetValues(typeof(EWeaponDescriptor)).Cast<EWeaponDescriptor>().ToList();

            for (int i = 0, i_max = provider.modifiersCount; i < i_max; i++)
            {
                int modifier_index = Random.Range(0, available_modifiers.Count);
                float value = Random.Range(provider.modificationRange.x, provider.modificationRange.y);
                
                switch (available_modifiers[modifier_index])
                {
                    case EWeaponDescriptor.Damage: modifiers.GetComponent<Damage>().value = value; break;
                    case EWeaponDescriptor.Accuracy: modifiers.GetComponent<Accuracy>().value = value; break;
                    case EWeaponDescriptor.FireRate: modifiers.GetComponent<FireRate>().value = value; break;
                    case EWeaponDescriptor.ClipSize: modifiers.GetComponent<ClipSize>().value = (uint)(value * 100f); break;
                    case EWeaponDescriptor.ReloadTime: modifiers.GetComponent<ReloadTime>().value = value; break;
                    default: break;
                }
                
                available_modifiers.RemoveAt(modifier_index);
            }

        }
    };
}
