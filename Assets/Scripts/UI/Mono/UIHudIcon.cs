using System;
using Scorewarrior.ECS;
using Scorewarrior.Test.Models;
using UnityEngine;
using UnityEngine.UI;
using UniversalEntities;

namespace Scorewarrior.UI
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
        
        public bool IsCompatibleWith(uint sector)
        {
            return (sector == Sector);
        }
        
        public void AttachCharacter(ICharacter character)
        {
            if (_attachedCharacter != null)
            {
                ClearCurrentCharacter();
            }
            
            _attachedCharacter = character;
            
            character.OnDamageTaken += OnCharacterDamageTaken;
            character.OnStateChanged += OnCharacterStateChanged;

            _canvasGroup.alpha = 1f;

            name = $"UIHud_{character.Prefab.GameObject.name}";
            
            UpdateBars();
        }

        public void ClearCurrentCharacter()
        {
            if (_attachedCharacter == null) return;
            
            _attachedCharacter.OnDamageTaken -= OnCharacterDamageTaken;
            _attachedCharacter.OnStateChanged -= OnCharacterStateChanged;
            
            _attachedCharacter = null;

            ClearBars();
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

        public void UpdateBars(Entity characterEntity)
        {
            var marker = characterEntity.GetComponent<CharacterMarker>();
            
            


        }
        
        public void ClearBars()
        {
            _canvasGroup.alpha = 0f;
            _armorBar.fillAmount = 1f;
            _healthBar.fillAmount = 1f;

            name = "UIHud_Is_EMPTY";
        }
        
        private void OnCharacterStateChanged(ICharacter obj)
        {
            if (obj.State == ECharacterState.Die)
            {
                ClearCurrentCharacter();
            }
        }
    }
}
