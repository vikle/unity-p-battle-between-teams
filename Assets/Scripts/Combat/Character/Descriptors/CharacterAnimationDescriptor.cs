using Scorewarrior.Test.Models;
using Scorewarrior.Test.Services;
using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior.Test.Descriptors
{
    [RequireComponent(typeof(CharacterPrefab))]
    public sealed class CharacterAnimationDescriptor : CachedMonoBehaviour
    {
        [Space]
        [SerializeField]Animator _animator;

        [Header("Parameters Names")]
        [SerializeField]string _aimingName = "aiming";
        [SerializeField]string _reloadingName = "reloading";
        [SerializeField]string _shootName = "shoot";
        [SerializeField]string _reloadTimeName = "reload_time";
        [SerializeField]string _dieName = "die";

        [Header("Settings")]
        [SerializeField]float _reloadAnimationLength = 3.3f;
        
        int _aimingHash;
        int _reloadingHash;
        int _shootHash;
        int _reloadTimeHash;
        int _dieHash;

        bool _aimingIsValid;
        bool _reloadingIsValid;
        bool _shootIsValid;
        bool _reloadTimeIsValid;
        bool _dieIsValid;
        
        ICharacterPrefab _characterPrefab;

        void Reset()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        void Awake()
        {
            InitComponents();
            InitAnimations();
        }
        
        private void InitComponents()
        {
            _characterPrefab = GetComponent<ICharacterPrefab>();
        }
        
        private void InitAnimations()
        {
            TryGetAnimationHash(_aimingName, out _aimingHash, out _aimingIsValid);
            TryGetAnimationHash(_reloadingName, out _reloadingHash, out _reloadingIsValid);
            TryGetAnimationHash(_shootName, out _shootHash, out _shootIsValid);
            TryGetAnimationHash(_reloadTimeName, out _reloadTimeHash, out _reloadTimeIsValid);
            TryGetAnimationHash(_dieName, out _dieHash, out _dieIsValid);
        }

        public override void OnUpdate(float deltaTime)
        {
            var characterState = _characterPrefab.Model.State;
            UpdateCharacterAnimations(characterState);
        }

        private void UpdateCharacterAnimations(CharacterState state)
        {
            if (_aimingIsValid)
            {
                _animator.SetBool(_aimingHash, state is CharacterState.Aiming 
                                      or CharacterState.TryShooting 
                                      or CharacterState.ShootFire 
                                      or CharacterState.Reloading);
            }

            if (_reloadingIsValid)
            {
                _animator.SetBool(_reloadingHash, state is CharacterState.Reloading);
            }
            
            switch (state)
            {
                case CharacterState.ShootFire:
                    if (_shootIsValid)
                    {
                        _animator.SetTrigger(_shootHash);
                    }
                    break;
                case CharacterState.Reloading: 
                    if (_reloadTimeIsValid)
                    {
                        float reloadTime = _characterPrefab.Weapon.Descriptor.ReloadTime;
                        _animator.SetFloat(_reloadTimeHash, reloadTime / _reloadAnimationLength);
                    }
                    break;
                case CharacterState.Die: 
                    if (_dieIsValid)
                    {
                        _animator.SetTrigger(_dieHash);
                    }
                    break;
                default: break;
            }
        }

        private static void TryGetAnimationHash(string animName, out int animHash, out bool isValid)
        {
            if (string.IsNullOrEmpty(animName))
            {
                isValid = false;
                animHash = 0;
            }

            isValid = true;
            animHash = Animator.StringToHash(animName);
        }
    }
}
