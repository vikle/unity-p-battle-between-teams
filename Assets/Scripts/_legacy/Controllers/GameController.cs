using System;
using Scorewarrior.Test.Models;
using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior.Test.Controllers
{
    [DefaultExecutionOrder(-1)]
    public sealed class GameController : MonoBehaviour
    {
        public static event Action<EGameState> OnGameStateChanged = delegate { };
        public static EGameState GameState { get; private set; }

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
            SwitchGameState(EGameState.Initiated, true);
        }

        public static void Wait()
        {
            if (GameState != EGameState.Initiated)
            {
                throw new Exception("Wait must be called only Initiated GameState");
            }
            
            Waiters++;
        }

        public static void Free()
        {
            if (GameState != EGameState.Starting)
            {
                throw new Exception("Free must be called only Starting GameState");
            }
            
            Waiters--;
            if (Waiters == 0)
            {
                SwitchGameState(EGameState.Started);
            }
        }
        
        public static void StartGame()
        {
            if (GameState != EGameState.Initiated)
            {
                throw new Exception("Game has been started");
            }
            
            SwitchGameState(Waiters > 0 ? EGameState.Starting : EGameState.Started);
        }
        
        public static void RestartGame()
        {
            SwitchGameState(EGameState.Initiated);
        }

        private static void SwitchGameState(EGameState newState, bool force = false)
        {
            if (!force && GameState == newState) return;
            GameState = newState;
            OnGameStateChanged.Invoke(newState);
        }

        private static void DoSwitchGameState(EGameState newState)
        {
            switch (newState)
            {
                case EGameState.Initiated: 
                    break;
                case EGameState.Starting:
                    break;
                case EGameState.Started:
                    break;
                case EGameState.Finished: 
                    break;
                default: break;
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static ICharacter SpawnCharacter(GameObject prefab, Vector3 position, IBattlefield battlefield, ETeam team, uint sector)
        {
            bool is_first_instance = !GameObjectPool.ContainsPrefab(prefab);
            
            var prefab_object = GameObjectPool.Instantiate(prefab, position);
            var prefab_clone = prefab_object.GetComponent<ICharacterPrefab>();

            if (is_first_instance)
            {
                prefab_clone.Init();
            }
            
            var chr_model = prefab_clone.Model;
            
            chr_model.Init(battlefield, team, sector);

            battlefield.RegisterCharacter(chr_model);
            
            if (is_first_instance)
            {
                chr_model.OnDamageTaken += DoCharacterDamageTaken;
                chr_model.OnStateChanged += DoCharacterStateChanged;
            }
            
            OnCharacterSpawned.Invoke(chr_model);
            return chr_model;
        }

        private static void DoCharacterStateChanged(ICharacter character)
        {
            OnCharacterStateChanged.Invoke(character);
            
            switch (character.State)
            {
                case ECharacterState.Idle: break;
                case ECharacterState.Aiming: break;
                case ECharacterState.TryShooting: break;
                case ECharacterState.ShootFire: break;
                case ECharacterState.Reloading: break;
                
                case ECharacterState.Die:
                    if (character.Battlefield.TeamIsDead(character.Team))
                    {
                        SwitchGameState(EGameState.Finished);
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
