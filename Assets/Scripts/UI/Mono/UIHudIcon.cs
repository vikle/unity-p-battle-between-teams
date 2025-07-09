using UnityEngine;
using UnityEngine.UI;
using UniversalEntities;

namespace Scorewarrior.UI
{
    using ECS;

    [RequireComponent(typeof(CanvasGroup))]
    public sealed class UIHudIcon : MonoBehaviour
    {
        public uint Sector { get; set; }
        [SerializeField]CanvasGroup _canvasGroup;
        [SerializeField]Image _armorBar;
        [SerializeField]Image _healthBar;

        Entity m_attachedCharacter;

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
    };
}
