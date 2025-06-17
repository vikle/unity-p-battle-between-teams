using System;
using System.Linq;
using UnityEngine;
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
    public sealed class CharacterAddModifiersSystem : IEntityInitializeSystem
    {
        [Preserve]public CharacterAddModifiersSystem(Pipeline pipeline) { }
        
        public void OnAfterEntityCreated(Pipeline pipeline, Entity entity)
        {
            var provider = entity.GetComponent<ObjectRef<GameObject>>()
                                 .Target.GetComponent<CharacterDescriptorProvider>();

            var modifiers_entity = entity.GetComponent<CharacterMarker>().modifiersEntity;
            
            modifiers_entity.AddComponent<Accuracy>().value = 1f;
            modifiers_entity.AddComponent<Dexterity>().value = 1f;
            modifiers_entity.AddComponent<Health>().value = 1f;
            modifiers_entity.AddComponent<Armor>().value = 1f;
            modifiers_entity.AddComponent<AimTime>().value = 1f;

            var available_modifiers = Enum.GetValues(typeof(ECharacterDescriptor)).Cast<ECharacterDescriptor>().ToList();
            
            for (int i = 0, i_max = provider.modifiersCount; i < i_max; i++)
            {
                int modifier_index = Random.Range(0, available_modifiers.Count);
                var modifier = available_modifiers[modifier_index];
                float value = Random.Range(provider.modificationRange.x, provider.modificationRange.y);
                
                switch (modifier)
                {
                    case ECharacterDescriptor.Accuracy: modifiers_entity.GetComponent<Accuracy>().value = value; break;
                    case ECharacterDescriptor.Dexterity: modifiers_entity.GetComponent<Dexterity>().value = value; break;
                    case ECharacterDescriptor.MaxHealth: modifiers_entity.GetComponent<Health>().value = value; break;
                    case ECharacterDescriptor.MaxArmor: modifiers_entity.GetComponent<Armor>().value = value; break;
                    case ECharacterDescriptor.AimTime: modifiers_entity.GetComponent<AimTime>().value = value; break;
                    default: break;
                }
                
                available_modifiers.RemoveAt(modifier_index);
            }
        }
    };
}
