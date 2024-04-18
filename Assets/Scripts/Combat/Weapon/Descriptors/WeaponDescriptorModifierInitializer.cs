using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scorewarrior.Test.Descriptors
{
    [RequireComponent(typeof(WeaponDescriptorProvider))]
    [DisallowMultipleComponent]
    public sealed class WeaponDescriptorModifierInitializer : MonoBehaviour
    {
        public Vector2 modificationRange = new Vector2(1f, 2f);
        public int modifiersCount = 2;
        
        void OnEnable()
        {
            var provider = GetComponent<WeaponDescriptorProvider>();
            provider.modifiers.Clear();

            var avaliableModifiers = Enum.GetValues(typeof(WeaponDescriptorValue))
                                         .Cast<WeaponDescriptorValue>()
                                         .ToList();

            for (int i = 0, i_max = modifiersCount; i < i_max; i++)
            {
                int randomIndex = Random.Range(0, avaliableModifiers.Count);

                provider.modifiers.Add(new()
                {
                    Descriptor = avaliableModifiers[randomIndex],
                    Value = Random.Range(modificationRange.x, modificationRange.y)
                });

                avaliableModifiers.RemoveAt(randomIndex);
            }
        }
    }
}
