using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
    public interface IWeaponPrefab
    {
        IWeaponDescriptorProvider Descriptor { get; }
        IWeapon Model { get; }
        Transform BarrelTransform { get; }
        GameObject ProjectilePrefab { get; }
        
        void Init();
    }
}
