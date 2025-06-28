using System;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class ScorewarriorBootstrap : UniversalEntitiesBootstrap
    { 
        public override void OnBootstrap(Pipeline pipeline)
        {
            DIContainer.Register(pipeline);
            
            pipeline
                .BindSystem<GameStateInitializeSystem>()
                .BindSystem<GameStateStartingSystem>()
                .BindSystem<CharacterSpawnSystem>()
                
                .BindSystem<FindNearestEnemyRequestHandleSystem>()
                ;
            
            pipeline
                .BindSystem<CharacterAddStatsSystem>()
                .BindSystem<CharacterAddModifiersSystem>()
                .BindSystem<WeaponAddStatsSystem>()
                .BindSystem<WeaponAddModifiersSystem>()
                ;
            
            pipeline
                .BindPromise<FindNearestEnemyRequest>()
                ;

            pipeline
                .BindEvent<GameStateChanged>()
                .BindEvent<CharacterSpawnTask>()
                .BindEvent<CharacterSpawned>()
                ;
        }

        void Start()
        {
            DIContainer.Resolve<GameController>().PrepareToStartGame();
        }
    };
}
