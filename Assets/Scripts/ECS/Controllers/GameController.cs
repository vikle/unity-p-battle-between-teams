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
            
        }
        
        public void RestartGame()
        {
            SwitchGameState(EGameState.Initiated);
        }
        
        
        private void SwitchGameState(EGameState newState, bool force = false)
        {
            if (GameState == newState && !force) return;
            GameState = newState;
            m_pipeline.Trigger<GameStateChanged>().newState = newState;
        }



        public void SpawnCharacter(GameObject prefab, Vector3 position, ETeam team, uint sector)
        {
            
            
            var prefab_clone = GameObjectPool.Instantiate(prefab, position);
            
            var actor = prefab_clone.GetComponent<EntityActor>();
            actor.InitEntity();

            var character_entity = actor.EntityRef;

            var meta_entity = character_entity.GetComponent<CharacterMarker>().metaEntity;
            meta_entity.GetComponent<Team>().value = team;
            meta_entity.GetComponent<Sector>().value = sector;
            meta_entity.GetComponent<CharacterState>().value = ECharacterState.Idle;

            m_pipeline.Trigger<CharacterSpawned>().characterEntity = character_entity;
        }
        
        
    };
}
