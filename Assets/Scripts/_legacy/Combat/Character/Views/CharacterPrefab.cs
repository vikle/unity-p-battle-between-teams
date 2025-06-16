using Scorewarrior.Test.Controllers;
using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Models;
using Scorewarrior.Test.Services;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterDescriptorProvider))]
    public sealed class CharacterPrefab : CachedMonoBehaviour, ICharacterPrefab
    {
        [SerializeField]CharacterDescriptorProvider _descriptor;
        [SerializeField]Transform _hitBox;
        [SerializeField]GameObject _weaponPrefab;
        [SerializeField]WeaponAttachment[] _attachments;

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
            checkPrefab(ref _weaponPrefab);
            
            for (int i = 0, i_max = _attachments.Length; i < i_max; i++)
            {
                ref var attachment = ref _attachments[i];
                checkPrefab(ref attachment.prefab);
            }

            return;
            
            static void checkPrefab(ref GameObject prefab)
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
            var weaponInstance = Instantiate(_weaponPrefab);

            var weapTr = weaponInstance.transform;
            var attachmentSlot = default(Transform);
            
            for (int i = 0, i_max = _attachments.Length; i < i_max; i++)
            {
                ref readonly var attachment = ref _attachments[i];
                if (attachment.prefab != _weaponPrefab) continue;
                attachmentSlot = attachment.slot;
                break;
            }

            if (attachmentSlot == null)
            {
                Debug.LogError($"Weapon attachment slot not found in '{name}'");
            }

            weapTr.SetParent(attachmentSlot);
            
            weapTr.localPosition = Vector3.zero;
            weapTr.localRotation = Quaternion.identity;

            Weapon = weaponInstance.GetComponent<IWeaponPrefab>();
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
