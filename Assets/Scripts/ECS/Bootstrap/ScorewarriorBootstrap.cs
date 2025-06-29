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
                
                .BindSystem<CharacterStateBehaviourSystem>()
                
                .BindSystem<FindNearestEnemyRequestHandleSystem>()
                ;
            
            pipeline
                .BindSystem<CharacterAddStatsSystem>()
                .BindSystem<CharacterAddModifiersSystem>()
                .BindSystem<CharacterAddMetaSystem>()
                .BindSystem<WeaponAddStatsSystem>()
                .BindSystem<WeaponAddModifiersSystem>()
                .BindSystem<WeaponAddMetaSystem>()
                ;

            pipeline
                .BindSystem<UIControllerUpdateSystem>()
                .BindSystem<UIHudUpdateSystem>()
                ;
            
            pipeline
                .BindPromise<FindNearestEnemyRequest>()
                ;

            pipeline
                .BindEvent<GameStateChanged>()
                .BindEvent<CharacterSpawnTask>()
                .BindEvent<CharacterSpawned>()
                .BindEvent<CharacterStateChanged>()
                ;
        }

        void Start()
        {
            DIContainer.Resolve<GameController>().Bootstrap();
        }
    };
}
