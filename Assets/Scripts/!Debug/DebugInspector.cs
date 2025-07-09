using System.Collections.Generic;
using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.DebugGame
{
    using ECS;

    public sealed class DebugInspector : MonoBehaviour
    {
        public EGameState m_state;
        public List<DebugViewCharacter> m_debugViewCharacters = new(8);

        DebugInspectorModel m_model;

        void Awake()
        {
            m_debugViewCharacters.Clear();
            
            DIContainer.Resolve(out m_model);
            m_model.onGameStateChanged = OnGameStateChanged;
            m_model.onCharacterSpawned = OnCharacterSpawned;
            m_model.onCharacterStateChanged = OnCharacterStateChanged;
            m_model.onCharacterDamageTaken = OnCharacterDamageTaken;
        }

        void OnDestroy()
        {
            m_model.Dispose();
        }

        private void OnGameStateChanged(EGameState newState)
        {
            m_state = newState;
        }

        private void OnCharacterSpawned(Entity character)
        {
            int view_index = m_debugViewCharacters.FindIndexNonAlloc(character, (view, value) => view.character == value);

            var meta = character.GetComponent<CharacterMarker>().meta;

            var view = new DebugViewCharacter
            {
                character = character,
                instance = character.GetComponent<ObjectRef<GameObject>>().Target,
                state = meta.GetComponent<CharacterState>().value,
                team = meta.GetComponent<Team>().value,
                sector = meta.GetComponent<Sector>().value,
                armor = (int)meta.GetComponent<Armor>().value,
                health = (int)meta.GetComponent<Health>().value,
            };

            if (view_index > -1)
            {
                m_debugViewCharacters[view_index] = view;
            }
            else
            {
                m_debugViewCharacters.Add(view);
            }
        }

        private void OnCharacterStateChanged(Entity character)
        {
            int view_index = m_debugViewCharacters.FindIndexNonAlloc(character, (view, value) => view.character == value);

            if (view_index < 0) return;

            var view = m_debugViewCharacters[view_index];

            var meta = character.GetComponent<CharacterMarker>().meta;

            view.state = meta.GetComponent<CharacterState>().value;
        }

        private void OnCharacterDamageTaken(Entity character)
        {
            int view_index = m_debugViewCharacters.FindIndexNonAlloc(character, (view, value) => view.character == value);

            if (view_index < 0) return;
            
            var view = m_debugViewCharacters[view_index];
            
            var meta = character.GetComponent<CharacterMarker>().meta;
            
            view.armor = (int)meta.GetComponent<Armor>().value;
            view.health = (int)meta.GetComponent<Health>().value;
        }
    };
}
