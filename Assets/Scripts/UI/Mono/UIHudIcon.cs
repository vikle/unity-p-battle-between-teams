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
        
        Entity m_attachedCharacter;
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
        
        public bool IsCompatibleSector(uint sector)
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

        public void AttachCharacter(Entity characterEntity)
        {
            if (m_attachedCharacter != null)
            {
                Dispose();
            }
            
            m_attachedCharacter = characterEntity;
            
            _canvasGroup.alpha = 1f;
            
            name = $"UIHud_{characterEntity.GetComponent<ObjectRef<GameObject>>().Target.name}";
            
            TryUpdateBars(characterEntity);
        }
        
        public void ClearCurrentCharacter()
        {
            if (_attachedCharacter == null) return;
            
            _attachedCharacter.OnDamageTaken -= OnCharacterDamageTaken;
            _attachedCharacter.OnStateChanged -= OnCharacterStateChanged;
            
            _attachedCharacter = null;

            Dispose();
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

        public void TryUpdateBars(Entity characterEntity)
        {
            if (m_attachedCharacter != characterEntity) return;
            
            var marker = characterEntity.GetComponent<CharacterMarker>();
            
            UpdateArmor(marker);
            UpdateHealth(marker);
        }

        private void UpdateArmor(CharacterMarker marker)
        {
            var current = marker.meta.GetComponent<Armor>();
            var max = marker.stats.GetComponent<Armor>();
            _armorBar.fillAmount = (current.value / max.value);
        }
        
        private void UpdateHealth(CharacterMarker marker)
        {
            var current = marker.meta.GetComponent<Health>();
            var max = marker.stats.GetComponent<Health>();
            _healthBar.fillAmount = (current.value / max.value);
        }

        public void TryDispose(Entity characterEntity)
        {
            if (m_attachedCharacter == characterEntity)
            {
                Dispose();
            }
        }

        private void Dispose()
        {
            m_attachedCharacter = null;
            
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
