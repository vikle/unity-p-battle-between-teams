using System.Collections.Generic;
using UnityEngine;

namespace Scorewarrior.Test.Descriptors
{
    [RequireComponent(typeof(WeaponDescriptorProvider))] 
    [DisallowMultipleComponent]
    public sealed class WeaponDescriptorProvider : MonoBehaviour, IWeaponDescriptorProvider
    {
        [SerializeField]WeaponDescriptor _descriptor;
        public List<WeaponDescriptorModifier> modifiers = new(8);
        
        public float Damage => _descriptor.Damage * GetModifierValue(WeaponDescriptorValue.Damage);
        public float Accuracy => _descriptor.Accuracy * GetModifierValue(WeaponDescriptorValue.Accuracy);
        public float FireRate => _descriptor.FireRate * GetModifierValue(WeaponDescriptorValue.FireRate);
        public uint ClipSize => (uint)(_descriptor.ClipSize * GetModifierValue(WeaponDescriptorValue.ClipSize));
        public float ReloadTime => _descriptor.ReloadTime * GetModifierValue(WeaponDescriptorValue.ReloadTime);
     
        void Reset()
        {
            _descriptor = GetComponent<WeaponDescriptor>();
        }
        
        private float GetModifierValue(WeaponDescriptorValue descriptorValue)
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
