using System;
using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class GameController
    {
        public EGameState GameState { get; private set; }
        
        readonly Pipeline m_pipeline;

        public GameController()
        {
            DIContainer.TryGet(out m_pipeline);
        }


        public void PrepareToStartGame()
        {
            if (GameState != EGameState.Initiated)
            {
                throw new Exception("Game has been starting");
            }
            
            SwitchGameState(EGameState.Starting);
        }
        
        public void StartGame()
        {
            if (GameState != EGameState.Starting)
            {
                throw new Exception("Game has been started");
            }
            
            SwitchGameState(EGameState.Started);
        }
        
        public void RestartGame()
        {
            SwitchGameState(EGameState.Initiated);
        }
        
        
        private void SwitchGameState(EGameState newState, bool force = false)
        {
            if (GameState == newState && !force) return;
            GameState = newState;
            m_pipeline.Trigger<GameStateChanged>().value = newState;
        }

        
        
    };
}
