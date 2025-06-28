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
            
            var modifiers_entity = entity.GetComponent<WeaponMarker>().modifiersEntity;
            var provider = entity.GetComponent<ObjectRef<WeaponDescriptorProvider>>().Target;

            modifiers_entity.AddComponent<Damage>().value = 1f;
            modifiers_entity.AddComponent<Accuracy>().value = 1f;
            modifiers_entity.AddComponent<FireRate>().value = 1f;
            modifiers_entity.AddComponent<ClipSize>().value = 1;
            modifiers_entity.AddComponent<ReloadTime>().value = 1f;

            var available_modifiers = Enum.GetValues(typeof(EWeaponDescriptor)).Cast<EWeaponDescriptor>().ToList();

            for (int i = 0, i_max = provider.modifiersCount; i < i_max; i++)
            {
                int modifier_index = Random.Range(0, available_modifiers.Count);
                var modifier = available_modifiers[modifier_index];
                float value = Random.Range(provider.modificationRange.x, provider.modificationRange.y);
                
                switch (modifier)
                {
                    case EWeaponDescriptor.Damage: modifiers_entity.GetComponent<Damage>().value = value; break;
                    case EWeaponDescriptor.Accuracy: modifiers_entity.GetComponent<Accuracy>().value = value; break;
                    case EWeaponDescriptor.FireRate: modifiers_entity.GetComponent<FireRate>().value = value; break;
                    case EWeaponDescriptor.ClipSize: modifiers_entity.GetComponent<ClipSize>().value = (uint)(value * 100f); break;
                    case EWeaponDescriptor.ReloadTime: modifiers_entity.GetComponent<ReloadTime>().value = value; break;
                    default: break;
                }
                
                available_modifiers.RemoveAt(modifier_index);
            }

        }
    };
}
