using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.UI
{
    using ECS;

    public sealed class UIHud : MonoBehaviour
    {
        [SerializeField]UIHudPanel[] _uiHudPanels;

        UIHudModel m_model;

        void Awake()
        {
            DIContainer.Resolve(out m_model);
            m_model.onCharacterSpawned = OnCharacterSpawned;
            m_model.onCharacterDamageTaken = OnCharacterDamageTaken;
            m_model.onCharacterDie = OnCharacterDie;
        }

        void OnDestroy()
        {
            m_model.Dispose();
        }

        void Reset()
        {
            _uiHudPanels = GetComponentsInChildren<UIHudPanel>();
        }

        private void OnCharacterDamageTaken(Entity characterEntity)
        {
            foreach (var panel in _uiHudPanels)
            {
                panel.OnCharacterDamageTaken(characterEntity);
            }
        }

        private void OnCharacterDie(Entity characterEntity)
        {
            foreach (var panel in _uiHudPanels)
            {
                panel.OnCharacterDie(characterEntity);
            }
        }

        private void OnCharacterSpawned(Entity characterEntity)
        {
            GetTeamAndSector(characterEntity, out var team, out uint sector);

            foreach (var panel in _uiHudPanels)
            {
                if (!panel.IsCompatibleTeam(team)) continue;
                if (!panel.TryAttachCharacter(characterEntity, sector)) continue;
                break;
            }
        }

        private static void GetTeamAndSector(Entity characterEntity, out ETeam team, out uint sector)
        {
            var marker = characterEntity.GetComponent<CharacterMarker>();
            team = marker.meta.GetComponent<Team>().value;
            sector = marker.meta.GetComponent<Sector>().value;
        }
    };
}
