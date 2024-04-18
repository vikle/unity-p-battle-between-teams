using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior.Test.Models
{
    public interface IWeapon
    {
        public bool IsReady { get; }
        public bool HasAmmo { get; }
        IWeaponPrefab Prefab { get; }

        void Reload();
        void Fire(IDamageable target, Vector3 targetPosition, bool hit);
    }
}
