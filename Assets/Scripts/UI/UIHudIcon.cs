using System;
using Scorewarrior.Test.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Scorewarrior.Test.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class UIHudIcon : MonoBehaviour
    {
        public uint Sector { get; set; }
        [SerializeField]CanvasGroup _canvasGroup;
        [SerializeField]Image _armorBar;
        [SerializeField]Image _healthBar;

        ICharacter _attachedCharacter;

        void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.ignoreParentGroups = false;
        }

        void Awake()
        {
            _canvasGroup.alpha = 0f;
        }

        public void SetHealthBarColor(Color color)
        {
            _healthBar.color = color;
        }
        
        public bool IsCompatibleCharacter(ICharacter character)
        {
            return (character.Sector == Sector);
        }
        
        public void AttachCharacter(ICharacter character)
        {
            if (_attachedCharacter != null)
            {
                DeAttachCurrentCharacter();
            }
            
            _attachedCharacter = character;
            
            character.OnDamageTaken += OnCharacterDamageTaken;
            character.OnStateChanged += OnCharacterStateChanged;

            _canvasGroup.alpha = 1f;

            name = $"UIHud_{character.Prefab.GameObject.name}";
            
            UpdateBars();
        }

        public void DeAttachCurrentCharacter()
        {
            if (_attachedCharacter == null) return;
            
            _attachedCharacter.OnDamageTaken -= OnCharacterDamageTaken;
            _attachedCharacter.OnStateChanged -= OnCharacterStateChanged;
            
            _attachedCharacter = null;
            _canvasGroup.alpha = 0f;
            _armorBar.fillAmount = 1f;
            _healthBar.fillAmount = 1f;

            name = "UIHud_Is_EMPTY";
        }
        
        private void OnCharacterDamageTaken(ICharacter obj)
        {
            UpdateBars();
        }

        private void UpdateBars()
        {
            var desc = _attachedCharacter.Prefab.Descriptor;
            _armorBar.fillAmount = (_attachedCharacter.Armor / desc.MaxArmor);
            _healthBar.fillAmount = (_attachedCharacter.Health / desc.MaxHealth);
        }
        
        private void OnCharacterStateChanged(ICharacter obj)
        {
            if (obj.State == ECharacterState.Die)
            {
                DeAttachCurrentCharacter();
            }
        }
    }
}
