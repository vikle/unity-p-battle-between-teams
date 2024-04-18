using System;
using Scorewarrior.Test.Controllers;
using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.UI
{
    public sealed class UIController : MonoBehaviour
    {
        [SerializeField]GameObject _continueBtn;
        [SerializeField]GameObject _replayBtn;
        [SerializeField]UIHud _uiHud;
        
        void Awake()
        {
            GameController.OnGameStateChanged += OnGameStateChanged;
            GameController.OnCharacterSpawned += OnCharacterSpawned;
            GameController.OnCharacterDamageTaken += OnCharacterDamageTaken;
            GameController.OnCharacterDie += OnCharacterDie;
        }

        private void OnGameStateChanged(GameState newState)
        {
            _continueBtn.SetActive(newState == GameState.Initiated);
            _replayBtn.SetActive(newState == GameState.Finished);
            _uiHud.gameObject.SetActive(newState == GameState.Started);
            
            switch (newState)
            {
                case GameState.Initiated: 
                    break;
                case GameState.Started:
                    InitHud();
                    break;
                case GameState.Finished: 
                    break;
                default: break;
            }
        }

        private void OnCharacterSpawned(ICharacter character)
        {
            _uiHud.AttachCharacter(character);
        }

        private void OnCharacterDamageTaken(ICharacter damageable)
        {
            
        }

        private void OnCharacterDie(ICharacter damageable)
        {
            
        }
        
        public void OnContinueClick()
        {
            GameController.StartGame();
        }
        
        public void OnReplayClick()
        {
            GameController.RestartGame();
        }

        private void InitHud()
        {
            
        }
    }
}
