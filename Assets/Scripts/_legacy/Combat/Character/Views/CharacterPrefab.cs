using Scorewarrior.Test.Controllers;
using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Models;
using Scorewarrior.Test.Services;
using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterDescriptorProvider))]
    public sealed class CharacterPrefab : CachedMonoBehaviour, ICharacterPrefab
    {
        [SerializeField]CharacterDescriptorProvider _descriptor;
        
        public Transform _hitBox;
        public GameObject _weaponPrefab;
        public Transform _weaponSlot;

        Character _characterModel;

        public ICharacterDescriptorProvider Descriptor => _descriptor;
        public ICharacter Model => _characterModel;
        public GameObject GameObject => gameObject;
        public Transform Transform { get; private set; }
        
        Vector3 IDamageablePrefab.HitBoxPosition => _hitBox.position;
        
        public IWeaponPrefab Weapon { get; private set; }
        

#if UNITY_EDITOR
        void Reset()
        {
            _descriptor = GetComponent<CharacterDescriptorProvider>();
        }

        void OnValidate()
        {
            check_weapon_prefab(ref _weaponPrefab);

            return;
            
            static void check_weapon_prefab(ref GameObject prefab)
            {
                if (prefab == null) return;
                if (prefab.GetComponent<IWeaponPrefab>() != null) return;
                prefab = null;
                Debug.LogError("IWeaponPrefab must be implemented");
            }
        }
#endif

        public void Init()
        {
            InitWeapon();
            Transform = transform;
            _characterModel = new Character(this);
        }
        
        private void InitWeapon()
        {
            if (_weaponSlot == null)
            {
                Debug.LogError($"Weapon attachment slot not found in '{name}'");
            }

            var weapon_instance = Instantiate(_weaponPrefab);
            
            var weap_tr = weapon_instance.transform;
            weap_tr.SetParent(_weaponSlot);
            weap_tr.localPosition = Vector3.zero;
            weap_tr.localRotation = Quaternion.identity;

            Weapon = weapon_instance.GetComponent<IWeaponPrefab>();
            Weapon.Init();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (GameController.GameState == EGameState.Started)
            {
                _characterModel.Update(deltaTime);
            }
        }
    }
}
