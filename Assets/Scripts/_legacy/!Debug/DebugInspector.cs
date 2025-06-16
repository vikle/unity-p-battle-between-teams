using System.Collections.Generic;
using UnityEngine;
using Scorewarrior.Test.Controllers;
using Scorewarrior.Test.Models;

namespace Scorewarrior.Test._Debug
{
    public sealed class DebugInspector : MonoBehaviour
    {
        public int m_waiters;
        public EGameState m_state;
        public List<DebugViewCharacter> m_debugViewCharacters = new(8);
        
        static ICharacter _nonAllocCharacter;
        
        void Start()
        {
            m_debugViewCharacters.Clear();
            GameController.OnGameStateChanged += OnGameStateChanged;
            GameController.OnCharacterSpawned += OnCharacterSpawned;
            GameController.OnCharacterStateChanged += OnCharacterStateChanged;
        }

        void LateUpdate()
        {
            m_waiters = GameController.Waiters;
        }

        private void OnGameStateChanged(EGameState newState)
        {
            m_state = newState;
        }
        
        private void OnCharacterSpawned(ICharacter character)
        {
            _nonAllocCharacter = character;

            int viewIndex = m_debugViewCharacters.FindIndex(v => v.character == _nonAllocCharacter);
            
            if (viewIndex > -1)
            {
                m_debugViewCharacters[viewIndex].Team = character.Team;
                return;
            }
            
            var view = new DebugViewCharacter
            {
                character = character,
                Instance = character.Prefab.GameObject,
                Team = character.Team,
                Sector = character.Sector
            };
            
            m_debugViewCharacters.Add(view);
        }
        
        private void OnCharacterStateChanged(ICharacter character)
        {
            _nonAllocCharacter = character;
            
            int viewIndex = m_debugViewCharacters.FindIndex(v => v.character == _nonAllocCharacter);
            
            if (viewIndex < 0)
            {
                return;
            }

            m_debugViewCharacters[viewIndex].State = character.State;
        }
    }
}
