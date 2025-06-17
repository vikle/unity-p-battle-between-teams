using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scorewarrior.Test.Descriptors
{
    [RequireComponent(typeof(WeaponDescriptorProvider))] 
    [DisallowMultipleComponent]
    public sealed class WeaponDescriptorProvider : MonoBehaviour, IWeaponDescriptorProvider
    {
        [SerializeField]WeaponDescriptor _descriptor;
        
        public Vector2 modificationRange = new Vector2(1f, 2f);
        public int modifiersCount = 2;
        
        public List<WeaponDescriptorModifier> modifiers = new(8);

        void OnEnable()
        {
            modifiers.Clear();

            var available_modifiers = Enum.GetValues(typeof(EWeaponDescriptor)).Cast<EWeaponDescriptor>().ToList();

            for (int i = 0, i_max = modifiersCount; i < i_max; i++)
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

        public float Damage => _descriptor.Damage * GetModifierValue(EWeaponDescriptor.Damage);
        public float Accuracy => _descriptor.Accuracy * GetModifierValue(EWeaponDescriptor.Accuracy);
        public float FireRate => _descriptor.FireRate * GetModifierValue(EWeaponDescriptor.FireRate);
        public uint ClipSize => (uint)(_descriptor.ClipSize * GetModifierValue(EWeaponDescriptor.ClipSize));
        public float ReloadTime => _descriptor.ReloadTime * GetModifierValue(EWeaponDescriptor.ReloadTime);
     
        void Reset()
        {
            _descriptor = GetComponent<WeaponDescriptor>();
        }
        
        private float GetModifierValue(EWeaponDescriptor descriptorValue)
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
