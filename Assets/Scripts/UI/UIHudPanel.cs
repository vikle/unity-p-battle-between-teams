using System;
using System.Collections.Generic;
using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.UI
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
        
        public bool IsCompatibleCharacter(ICharacter character)
        {
            return (character.Team == _team);
        }

        static ICharacter _nonAllocCharacter;

        public bool TryAttachCharacter(ICharacter character)
        {
            _nonAllocCharacter = character;
            var foundedIcon = _preparedIcons.Find(icon => icon.IsCompatibleCharacter(_nonAllocCharacter));
            if (foundedIcon == null) return false;
            foundedIcon.AttachCharacter(character);
            return true;
        }
    }
}
