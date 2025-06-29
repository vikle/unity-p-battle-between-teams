using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.UI
{
    public sealed class UIController : MonoBehaviour
    {
        [SerializeField]GameObject _continueBtn;
        [SerializeField]GameObject _replayBtn;
        [SerializeField]UIHud _uiHud;

        ECS.GameController m_gameController;
        
        void Awake()
        {
            m_gameController = DIContainer.Resolve<ECS.GameController>();
            
            // Controllers.GameController.OnGameStateChanged += OnGameStateChanged;
            // Controllers.GameController.OnCharacterSpawned += OnCharacterSpawned;
            // Controllers.GameController.OnCharacterDamageTaken += OnCharacterDamageTaken;
            // Controllers.GameController.OnCharacterDie += OnCharacterDie;
        }

        public void OnGameStateChanged(EGameState newState)
        {
            _continueBtn.SetActive(newState == EGameState.Initiated);
            _replayBtn.SetActive(newState == EGameState.Finished);
            _uiHud.gameObject.SetActive(newState == EGameState.Started);
        }

        private void OnCharacterSpawned(ICharacter character)
        {
            _uiHud.AttachCharacter(character);
        }
        
        public void OnContinueClick()
        {
            // Controllers.GameController.StartGame();
            m_gameController.PrepareToStartGame();
        }
        
        public void OnReplayClick()
        {
            // Controllers.GameController.RestartGame();
            m_gameController.RestartGame();
        }
    }
}
