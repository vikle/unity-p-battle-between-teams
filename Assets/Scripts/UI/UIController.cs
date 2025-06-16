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

        private void OnGameStateChanged(EGameState newState)
        {
            _continueBtn.SetActive(newState == EGameState.Initiated);
            _replayBtn.SetActive(newState == EGameState.Finished);
            _uiHud.gameObject.SetActive(newState == EGameState.Started);
            
            switch (newState)
            {
                case EGameState.Initiated: 
                    break;
                case EGameState.Started:
                    InitHud();
                    break;
                case EGameState.Finished: 
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
