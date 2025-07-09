using UnityEngine;
using UnityEngine.Serialization;

namespace Scorewarrior
{
    [DisallowMultipleComponent]
    public sealed class WeaponPrefab : MonoBehaviour
    {
        [FormerlySerializedAs("BarrelTransform")]
        public Transform _barrelTransform;
        public GameObject projectilePrefab;
    };
}