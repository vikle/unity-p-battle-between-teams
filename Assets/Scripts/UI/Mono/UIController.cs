using UnityEngine;

namespace Scorewarrior.UI
{
    using ECS;
    
    public sealed class UIController : MonoBehaviour
    {
        [SerializeField]GameObject _continueBtn;
        [SerializeField]GameObject _replayBtn;
        [SerializeField]UIHud _uiHud;

        GameController m_gameController;
        UIControllerModel  m_model;
        
        void Awake()
        {
            DIContainer.Resolve(out m_gameController);
            DIContainer.Resolve(out m_model);

            m_model.onGameStateChanged = OnGameStateChanged;
        }

        void OnDestroy()
        {
            m_model.Dispose();
        }

        private void OnGameStateChanged(EGameState newState)
        {
            _continueBtn.SetActive(newState == EGameState.Initiated);
            _uiHud.gameObject.SetActive(newState == EGameState.Started);
            _replayBtn.SetActive(newState == EGameState.Finished);
        }
        
        public void OnContinueClick()
        {
            m_gameController.PrepareToStartGame();
        }
        
        public void OnReplayClick()
        {
            m_gameController.RestartGame();
        }
    }
}
