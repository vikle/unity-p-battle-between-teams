using System.Collections.Generic;
using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.UI
{
    public sealed class UIHudPanel : MonoBehaviour
    {
        [SerializeField]UIHudIcon _uiHudIcon;
        [SerializeField]Color _healthBarsColor = Color.green;
        [SerializeField]public ETeam _team;
        [SerializeField]uint[] _sectors;

        readonly List<UIHudIcon> _preparedIcons = new(8);

        void Awake()
        {
            _uiHudIcon.gameObject.SetActive(false);

            foreach (uint sector in _sectors)
            {
                var icon = Instantiate(_uiHudIcon, transform);
                icon.gameObject.SetActive(true);
                _preparedIcons.Add(icon);
                icon.Sector = sector;
                icon.SetHealthBarColor(_healthBarsColor);
            }
        }

        public bool IsCompatibleTeam(ETeam characterTeam)
        {
            return (characterTeam == _team);
        }

        public bool TryAttachCharacter(Entity characterEntity, uint sector)
        {
            foreach (var icon in _preparedIcons)
            {
                if (!icon.IsCompatibleSector(sector)) continue;
                icon.AttachCharacter(characterEntity);
                return true;
            }

            return false;
        }

        public void OnCharacterDamageTaken(Entity characterEntity)
        {
            foreach (var icon in _preparedIcons)
            {
                icon.TryUpdateBars(characterEntity);
            }
        }

        public void OnCharacterDie(Entity characterEntity)
        {
            foreach (var icon in _preparedIcons)
            {
                icon.TryDispose(characterEntity);
            }
        }
    };
}
