using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scorewarrior.Test.Descriptors
{
    [RequireComponent(typeof(CharacterDescriptor))]
    [DisallowMultipleComponent]
    public sealed class CharacterDescriptorProvider : MonoBehaviour, ICharacterDescriptorProvider
    {
        [SerializeField]CharacterDescriptor _descriptor;
        public List<CharacterDescriptorModifier> modifiers = new(8);

        public float Accuracy => _descriptor.Accuracy * GetModifierValue(CharacterDescriptorValue.Accuracy);
        public float Dexterity => _descriptor.Dexterity * GetModifierValue(CharacterDescriptorValue.Dexterity);
        public float MaxHealth => _descriptor.MaxHealth * GetModifierValue(CharacterDescriptorValue.MaxHealth);
        public float MaxArmor => _descriptor.MaxArmor * GetModifierValue(CharacterDescriptorValue.MaxArmor);
        public float AimTime => _descriptor.Accuracy * GetModifierValue(CharacterDescriptorValue.AimTime);

        void Reset()
        {
            _descriptor = GetComponent<CharacterDescriptor>();
        }

        private float GetModifierValue(CharacterDescriptorValue descriptorValue)
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
