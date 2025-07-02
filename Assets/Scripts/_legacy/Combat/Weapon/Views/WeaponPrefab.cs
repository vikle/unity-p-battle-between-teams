using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scorewarrior.Test.Views
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(WeaponDescriptorProvider))]
    public sealed class WeaponPrefab : MonoBehaviour, IWeaponPrefab
    {
        [SerializeField]WeaponDescriptorProvider _descriptor;
        
        [FormerlySerializedAs("BarrelTransform")]public Transform _barrelTransform;
        public GameObject projectilePrefab;

        public IWeaponDescriptorProvider Descriptor => _descriptor;
        public IWeapon Model { get; private set; }
        public Transform BarrelTransform => _barrelTransform;
        public GameObject ProjectilePrefab => projectilePrefab;

        void Reset()
        {
            _descriptor = GetComponent<WeaponDescriptorProvider>();
        }

        public void Init()
        {
            Model = new Weapon(this);
        }
	}
}