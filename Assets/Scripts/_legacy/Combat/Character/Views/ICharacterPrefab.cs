using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
    public interface ICharacterPrefab : IDamageablePrefab
    {
        ICharacterDescriptorProvider Descriptor { get; }
        ICharacter Model { get; }
        GameObject GameObject { get; }
        Transform Transform { get; }
        IWeaponPrefab Weapon { get; }
        
        void Init();
    }
}
