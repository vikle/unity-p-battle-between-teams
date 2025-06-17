using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scorewarrior.Test.Descriptors
{
    [RequireComponent(typeof(CharacterDescriptor))]
    [DisallowMultipleComponent]
    public sealed class CharacterDescriptorProvider : MonoBehaviour, ICharacterDescriptorProvider
    {
        [SerializeField]CharacterDescriptor _descriptor;
        
        public Vector2 modificationRange = new Vector2(1f, 2f);
        public int modifiersCount = 3;
        public List<CharacterDescriptorModifier> modifiers = new(8);
        
        void OnEnable()
        {
            modifiers.Clear();

            var available_modifiers = Enum.GetValues(typeof(ECharacterDescriptor)).Cast<ECharacterDescriptor>().ToList();

            for (int i = 0; i < modifiersCount; i++)
            {
                int modifier_index = Random.Range(0, available_modifiers.Count);

                modifiers.Add(new()
                {
                    Descriptor = available_modifiers[modifier_index],
                    Value = Random.Range(modificationRange.x, modificationRange.y)
                });

                available_modifiers.RemoveAt(modifier_index);
            }
        }

        public float Accuracy => _descriptor.Accuracy * GetModifierValue(ECharacterDescriptor.Accuracy);
        public float Dexterity => _descriptor.Dexterity * GetModifierValue(ECharacterDescriptor.Dexterity);
        public float MaxHealth => _descriptor.MaxHealth * GetModifierValue(ECharacterDescriptor.MaxHealth);
        public float MaxArmor => _descriptor.MaxArmor * GetModifierValue(ECharacterDescriptor.MaxArmor);
        public float AimTime => _descriptor.Accuracy * GetModifierValue(ECharacterDescriptor.AimTime);

        void Reset()
        {
            _descriptor = GetComponent<CharacterDescriptor>();
        }

        private float GetModifierValue(ECharacterDescriptor descriptorValue)
        {
            for (int i = 0, i_max = modifiers.Count; i < i_max; i++)
            {
                var modifier = modifiers[i];
                if (modifier.Descriptor == descriptorValue)
                {
                    return modifier.Value;
                }
            }

            return 1f;
        }
    }
}
