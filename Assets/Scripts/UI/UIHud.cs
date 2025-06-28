using System;
using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.UI
{
    public sealed class UIHud : MonoBehaviour
    {
        [SerializeField]UIHudPanel[] _uiHudPanels;

        void Reset()
        {
            _uiHudPanels = GetComponentsInChildren<UIHudPanel>();
        }

        public void AttachCharacter(ICharacter character)
        {
            for (int i = 0, i_max = _uiHudPanels.Length; i < i_max; i++)
            {
                var panel = _uiHudPanels[i];
                if (!panel.IsCompatibleCharacter(character)) continue;
                if (!panel.TryAttachCharacter(character)) continue;
                break;
            }
        }
    }
}
