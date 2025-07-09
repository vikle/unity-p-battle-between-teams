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
    public sealed class CharacterAddModifiersSystem : IEntityInitializeSystem
    {
        [Preserve]public CharacterAddModifiersSystem(Pipeline pipeline) { }
        
        public void OnAfterEntityCreated(Pipeline pipeline, Entity entity)
        {
            if (!entity.HasComponent<CharacterMarker>())
            {
                return;
            }
            
            var modifiers = entity.GetComponent<CharacterMarker>().modifiers;
            var provider = entity.GetComponent<ObjectRef<CharacterDescriptorProvider>>().Target;

            const float k_default_value = 1f;
            
            modifiers.AddComponent<Accuracy>().value = k_default_value;
            modifiers.AddComponent<Dexterity>().value = k_default_value;
            modifiers.AddComponent<Health>().value = k_default_value;
            modifiers.AddComponent<Armor>().value = k_default_value;
            modifiers.AddComponent<AimTime>().value = k_default_value;

            var available_modifiers = Enum.GetValues(typeof(ECharacterStat)).Cast<ECharacterStat>().ToList();
            
            for (int i = 0, i_max = provider.modifiersCount; i < i_max; i++)
            {
                int modifier_index = Random.Range(0, available_modifiers.Count);
                float value = Random.Range(provider.modificationRange.x, provider.modificationRange.y);
                
                switch (available_modifiers[modifier_index])
                {
                    case ECharacterStat.Accuracy: modifiers.GetComponent<Accuracy>().value = value; break;
                    case ECharacterStat.Dexterity: modifiers.GetComponent<Dexterity>().value = value; break;
                    case ECharacterStat.MaxHealth: modifiers.GetComponent<Health>().value = value; break;
                    case ECharacterStat.MaxArmor: modifiers.GetComponent<Armor>().value = value; break;
                    case ECharacterStat.AimTime: modifiers.GetComponent<AimTime>().value = value; break;
                    default: break;
                }
                
                available_modifiers.RemoveAt(modifier_index);
            }
        }
    };
}
