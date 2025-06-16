using System.Collections.Generic;
using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior.Test.Models
{
	public sealed class Weapon : IWeapon
	{
        float _lastFireTime;
        uint _ammo;

        public bool IsReady => (_lastFireTime < Time.time);
        public bool HasAmmo => (_ammo > 0);
        
        public IWeaponPrefab Prefab { get; }
        
        static readonly Dictionary<GameObject, IBulletPrefab> _cachedBulletPrefabs = new(32);
        
        public Weapon(IWeaponPrefab prefab)
		{
            Prefab = prefab;
			_ammo = prefab.Descriptor.ClipSize;
		}

        public void Reload()
		{
			_ammo = Prefab.Descriptor.ClipSize;
		}

        // ReSharper disable Unity.PerformanceAnalysis
        public void Fire(IDamageable target, Vector3 targetPosition,  bool hit)
		{
			if (_ammo == 0) return;
            
            _ammo--;
            _lastFireTime = (Time.time + 1f / Prefab.Descriptor.FireRate);
            var barrelPosition = Prefab.BarrelTransform.position;
            
            var bulletInstance = GameObjectPool.Instantiate(Prefab.BulletPrefab, barrelPosition);
            
            if (!_cachedBulletPrefabs.TryGetValue(bulletInstance, out var bulletPrefab) || bulletPrefab == null)
            {
                bulletPrefab = bulletInstance.GetComponent<IBulletPrefab>();
                _cachedBulletPrefabs[bulletInstance] = bulletPrefab;
            }
            
            bulletPrefab.Init(Prefab.Descriptor.Damage, target, targetPosition, hit);
        }

        // Убираем апдейт с уменьшением времени каждый кард, т.к. дешевше записывать последнее время выстрела и сравнивать с нужным интервалом
	}
}