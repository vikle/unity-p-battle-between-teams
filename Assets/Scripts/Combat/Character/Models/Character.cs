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

        public void Init(IBattlefield battlefield, Team team, uint sector)
        {
            Battlefield = battlefield;
            Team = team;
            Sector = sector;
            
            Health = Prefab.Descriptor.MaxHealth;
            Armor = Prefab.Descriptor.MaxArmor;
            _currentTarget = null;

            State = CharacterState.Idle;
        }
        
        public bool IsAlive => (State != CharacterState.Die);
        
        public float Armor { get; private set; }
        
        public float Health { get; private set; }

        public ICharacterPrefab Prefab { get; }
        
        public CharacterState State { get; private set; }
        
        public IBattlefield Battlefield { get; private set; }
        
        // Добавляет это поле, чтобы иметь возможность знать к какой команду относится перс
        public Team Team { get; private set; }
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
                SwitchState(CharacterState.Die);
            }
            
            OnDamageTaken.Invoke(this);
        }
        
		public void Update(float deltaTime)
        {
            if (!IsAlive) return;

            var weaponModel = Prefab.Weapon.Model;
            bool targetIsValid = (_currentTarget != null && _currentTarget.IsAlive);
            
            switch (State)
            {
                case CharacterState.Idle:
                    if (Battlefield.TryGetNearestAliveEnemy(this, out var target))
                    {
                        _currentTarget = target;
                        _nextAimTime = (Time.time + Prefab.Descriptor.AimTime);
                        SwitchState(CharacterState.Aiming);
                        Prefab.Transform.LookAt(target.Prefab.Transform.position);
                    }
                    break;
                    
                case CharacterState.Aiming:
                    if (targetIsValid)
                    {
                        if (_nextAimTime <= Time.time)
                        {
                            SwitchState(CharacterState.TryShooting);
                        }
                    }
                    else
                    {
                        SwitchState(CharacterState.Idle);
                    }
                    break;
                    
                case CharacterState.TryShooting:
                case CharacterState.ShootFire:
                    if (targetIsValid)
                    {
                        if (weaponModel.HasAmmo)
                        {
                            if (weaponModel.IsReady)
                            {
                                float hitChance = Random.value;
                                    
                                bool hit = (hitChance <= Prefab.Descriptor.Accuracy
                                         && hitChance <= weaponModel.Prefab.Descriptor.Accuracy
                                         && hitChance >= _currentTarget.Prefab.Descriptor.Dexterity);
                                    
                                weaponModel.Fire(_currentTarget, _currentTarget.Prefab.HitBoxPosition, hit);
                                    
                                SwitchState(CharacterState.ShootFire);
                            }
                            else
                            {
                                SwitchState(CharacterState.TryShooting);
                            }
                        }
                        else
                        {
                            SwitchState(CharacterState.Reloading);
                            _nextAimTime = weaponModel.Prefab.Descriptor.ReloadTime;
                        }
                    }
                    else
                    {
                        SwitchState(CharacterState.Idle);
                    }
                    break;
                    
                case CharacterState.Reloading:
                    if (_nextAimTime <= Time.time)
                    {
                        SwitchState(targetIsValid ? CharacterState.TryShooting : CharacterState.Idle);
                        weaponModel.Reload();
                    }
                    break;
                
                default: break;
            }
        }

        private void SwitchState(CharacterState newState)
        {
            State = newState;
            OnStateChanged.Invoke(this);
        }
	}
}