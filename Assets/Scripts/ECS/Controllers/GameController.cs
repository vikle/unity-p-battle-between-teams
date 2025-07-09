using System;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class GameController
    {
        public EGameState GameState { get; private set; }
        
        readonly Pipeline m_pipeline;

        int m_waiters;

        public bool IsNeedWait => (m_waiters > 0);
        
        public GameController()
        {
            DIContainer.TryGet(out m_pipeline);
        }

        public void Bootstrap()
        {
            GameState = EGameState.Initiated;
            m_pipeline.Trigger<GameStateChanged>().value = EGameState.Initiated;
        }

        public void AddWaiter()
        {
            m_waiters++;
        }
        
        public void FreeWaiter()
        {
            m_waiters--;
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
        
        public void FinishGame()
        {
            if (GameState != EGameState.Started)
            {
                throw new Exception("Game has been stopped");
            }
            
            SwitchGameState(EGameState.Finished);
        }
        
        public void RestartGame()
        {
            SwitchGameState(EGameState.Initiated);
        }
        
        private void SwitchGameState(EGameState newState)
        {
            if (GameState == newState) return;
            GameState = newState;
            m_pipeline.Trigger<GameStateChanged>().value = newState;
        }
    };
}
