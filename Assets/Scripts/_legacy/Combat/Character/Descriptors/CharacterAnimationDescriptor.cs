using Scorewarrior.Test.Services;
using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior.Test.Descriptors
{
    [RequireComponent(typeof(CharacterPrefab))]
    public sealed class CharacterAnimationDescriptor : CachedMonoBehaviour
    {
        [Space]
        public Animator _animator;

        [Header("Parameters Names")]
        public string _aimingName = "aiming";
        public string _reloadingName = "reloading";
        public string _shootName = "shoot";
        public string _reloadTimeName = "reload_time";
        public string _dieName = "die";

        [Header("Settings")]
        public float _reloadAnimationLength = 3.3f;
        
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
            AnimationHashTool.Get(_aimingName, out _aimingHash, out _aimingIsValid);
            AnimationHashTool.Get(_reloadingName, out _reloadingHash, out _reloadingIsValid);
            AnimationHashTool.Get(_shootName, out _shootHash, out _shootIsValid);
            AnimationHashTool.Get(_reloadTimeName, out _reloadTimeHash, out _reloadTimeIsValid);
            AnimationHashTool.Get(_dieName, out _dieHash, out _dieIsValid);
        }

        public override void OnUpdate(float deltaTime)
        {
            var characterState = _characterPrefab.Model.State;
            UpdateCharacterAnimations(characterState);
        }

        private void UpdateCharacterAnimations(ECharacterState state)
        {
            if (_aimingIsValid)
            {
                _animator.SetBool(_aimingHash, state is ECharacterState.Aiming 
                                      or ECharacterState.TryShooting 
                                      or ECharacterState.ShootFire 
                                      or ECharacterState.Reloading);
            }

            if (_reloadingIsValid)
            {
                _animator.SetBool(_reloadingHash, state is ECharacterState.Reloading);
            }
            
            switch (state)
            {
                case ECharacterState.ShootFire:
                    if (_shootIsValid)
                    {
                        _animator.SetTrigger(_shootHash);
                    }
                    break;
                case ECharacterState.Reloading: 
                    if (_reloadTimeIsValid)
                    {
                        float reload_time = _characterPrefab.Weapon.Descriptor.ReloadTime;
                        _animator.SetFloat(_reloadTimeHash, reload_time / _reloadAnimationLength);
                    }
                    break;
                case ECharacterState.Die: 
                    if (_dieIsValid)
                    {
                        _animator.SetTrigger(_dieHash);
                    }
                    break;
                default: break;
            }
        }
    }
}
