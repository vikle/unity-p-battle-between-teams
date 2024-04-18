using System;
using Scorewarrior.Test.Models;
using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior.Test.Controllers
{
    [DefaultExecutionOrder(-1)]
    public sealed class GameController : MonoBehaviour
    {
        public static event Action<GameState> OnGameStateChanged = delegate { };
        public static GameState GameState { get; private set; }

        public static int Waiters { get; private set; }
        
        public static event Action<ICharacter> OnCharacterSpawned = delegate { };
        public static event Action<ICharacter> OnCharacterDamageTaken = delegate { };
        public static event Action<ICharacter> OnCharacterStateChanged = delegate { };
        public static event Action<ICharacter> OnCharacterDie = delegate { };

        void Awake()
        {
            OnGameStateChanged += DoSwitchGameState;
        }

        void Start()
        {
            SwitchGameState(GameState.Initiated, true);
        }

        public static void Wait()
        {
            if (GameState != GameState.Initiated)
            {
                throw new Exception("Wait must be called only Initiated GameState");
            }
            
            Waiters++;
        }

        public static void Free()
        {
            if (GameState != GameState.Starting)
            {
                throw new Exception("Free must be called only Starting GameState");
            }
            
            Waiters--;
            if (Waiters == 0)
            {
                SwitchGameState(GameState.Started);
            }
        }
        
        public static void StartGame()
        {
            if (GameState != GameState.Initiated)
            {
                throw new Exception("Game has been started");
            }
            
            SwitchGameState(Waiters > 0 ? GameState.Starting : GameState.Started);
        }
        
        public static void RestartGame()
        {
            SwitchGameState(GameState.Initiated);
        }

        private static void SwitchGameState(GameState newState, bool force = false)
        {
            if (!force && GameState == newState) return;
            GameState = newState;
            OnGameStateChanged.Invoke(newState);
        }

        private static void DoSwitchGameState(GameState newState)
        {
            switch (newState)
            {
                case GameState.Initiated: 
                    break;
                case GameState.Starting:
                    break;
                case GameState.Started:
                    break;
                case GameState.Finished: 
                    break;
                default: break;
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static ICharacter SpawnCharacter(GameObject prefab, Vector3 position, IBattlefield battlefield, Team team, uint sector)
        {
            bool isFirstInstance = !GameObjectPool.ContainsPrefab(prefab);
            
            var prefabObject = GameObjectPool.Instantiate(prefab, position);
            var prefabClone = prefabObject.GetComponent<ICharacterPrefab>();

            if (isFirstInstance)
            {
                prefabClone.Init();
            }
            
            var chrModel = prefabClone.Model;
            
            chrModel.Init(battlefield, team, sector);

            battlefield.RegisterCharacter(chrModel);
            
            if (isFirstInstance)
            {
                chrModel.OnDamageTaken += DoCharacterDamageTaken;
                chrModel.OnStateChanged += DoCharacterStateChanged;
            }
            
            OnCharacterSpawned.Invoke(chrModel);
            return chrModel;
        }

        private static void DoCharacterStateChanged(ICharacter character)
        {
            OnCharacterStateChanged.Invoke(character);
            
            switch (character.State)
            {
                case CharacterState.Idle: break;
                case CharacterState.Aiming: break;
                case CharacterState.TryShooting: break;
                case CharacterState.ShootFire: break;
                case CharacterState.Reloading: break;
                
                case CharacterState.Die:
                    if (character.Battlefield.TeamIsDead(character.Team))
                    {
                        SwitchGameState(GameState.Finished);
                    }
                    
                    OnCharacterDie.Invoke(character);
                    break;
                default: break;
            }
        }
        
        private static void DoCharacterDamageTaken(ICharacter character)
        {
            OnCharacterDamageTaken.Invoke(character);
        }
    }
}
