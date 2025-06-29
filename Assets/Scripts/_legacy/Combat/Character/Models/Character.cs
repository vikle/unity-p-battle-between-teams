using System;
using UnityEngine;
using Scorewarrior.Test.Views;
using Random = UnityEngine.Random;

namespace Scorewarrior.Test.Models
{
    public sealed class Character : ICharacter
	{
        ICharacter _currentTarget;
		float _nextAimTime;

		public Character(ICharacterPrefab prefab)
        {
            Prefab = prefab;
        }

        public void Init(IBattlefield battlefield, ETeam team, uint sector)
        {
            Battlefield = battlefield;
            Team = team;
            Sector = sector;
            
            Health = Prefab.Descriptor.MaxHealth;
            Armor = Prefab.Descriptor.MaxArmor;
            _currentTarget = null;

            State = ECharacterState.Idle;
        }
        
        public bool IsAlive => (State != ECharacterState.Die);
        
        public float Armor { get; private set; }
        
        public float Health { get; private set; }

        public ICharacterPrefab Prefab { get; }
        
        public ECharacterState State { get; private set; }
        
        public IBattlefield Battlefield { get; private set; }
        
        // Добавляет это поле, чтобы иметь возможность знать к какой команду относится перс
        public ETeam Team { get; private set; }
        public uint Sector { get; private set; }

        public event Action<ICharacter> OnDamageTaken = delegate { };
        public event Action<ICharacter> OnStateChanged = delegate { };

        public void TakeDamage(float damage)
        {
            if (!IsAlive) return;
            
            if (Armor > 0f)
            {
                Armor -= damage;
            }
            else if (Health > 0f)
            {
                Health -= damage;
            }
            else
            {
                SwitchState(ECharacterState.Die);
            }
            
            OnDamageTaken.Invoke(this);
        }
        
		public void Update(float deltaTime)
        {
            if (!IsAlive) return;

            var weaponModel = Prefab.Weapon.Model;
            bool target_is_valid = (_currentTarget != null && _currentTarget.IsAlive);
            
            switch (State)
            {
                case ECharacterState.Idle:
                    if (Battlefield.TryGetNearestAliveEnemy(this, out var target))
                    {
                        _currentTarget = target;
                        _nextAimTime = (Time.time + Prefab.Descriptor.AimTime);
                        SwitchState(ECharacterState.Aiming);
                        Prefab.Transform.LookAt(target.Prefab.Transform.position);
                    }
                    break;
                    
                case ECharacterState.Aiming:
                    if (target_is_valid)
                    {
                        if (_nextAimTime <= Time.time)
                        {
                            SwitchState(ECharacterState.TryShooting);
                        }
                    }
                    else
                    {
                        SwitchState(ECharacterState.Idle);
                    }
                    break;
                    
                case ECharacterState.TryShooting:
                case ECharacterState.ShootFire:
                    if (target_is_valid)
                    {
                        if (weaponModel.HasAmmo)
                        {
                            if (weaponModel.IsReady)
                            {
                                float hit_chance = Random.value;
                                    
                                bool hit = (hit_chance <= Prefab.Descriptor.Accuracy
                                         && hit_chance <= weaponModel.Prefab.Descriptor.Accuracy
                                         && hit_chance >= _currentTarget.Prefab.Descriptor.Dexterity);
                                    
                                weaponModel.Fire(_currentTarget, _currentTarget.Prefab.HitBoxPosition, hit);
                                    
                                SwitchState(ECharacterState.ShootFire);
                            }
                            else
                            {
                                SwitchState(ECharacterState.TryShooting);
                            }
                        }
                        else
                        {
                            SwitchState(ECharacterState.Reloading);
                            _nextAimTime = weaponModel.Prefab.Descriptor.ReloadTime;
                        }
                    }
                    else
                    {
                        SwitchState(ECharacterState.Idle);
                    }
                    break;
                    
                case ECharacterState.Reloading:
                    if (_nextAimTime <= Time.time)
                    {
                        var next_state = target_is_valid 
                            ? ECharacterState.TryShooting 
                            : ECharacterState.Idle;
                        
                        SwitchState(next_state);
                        
                        weaponModel.Reload();
                    }
                    break;
                
                default: break;
            }
        }

        private void SwitchState(ECharacterState newState)
        {
            State = newState;
            OnStateChanged.Invoke(this);
        }
	}
}