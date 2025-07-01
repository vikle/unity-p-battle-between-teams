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
                .BindSystem<SpawnCharacterSystem>()
                .BindSystem<SpawnCharacterWeaponSystem>()
                
                .BindSystem<CharacterStateBehaviourSystem>()
                
                .BindSystem<FindNearestEnemyRequestHandleSystem>()
                ;
            
            pipeline
                .BindSystem<CharacterAddModifiersSystem>()
                .BindSystem<CharacterAddStatsSystem>()
                .BindSystem<CharacterAddMetaSystem>()
                .BindSystem<WeaponAddModifiersSystem>()
                .BindSystem<WeaponAddStatsSystem>()
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
                .BindEvent<SpawnCharacterCommand>()
                .BindEvent<CharacterSpawned>()
                .BindEvent<CharacterStateChanged>()
                .BindEvent<WeaponFireCommand>()
                .BindEvent<WeaponReloadCommand>()
                ;
        }

        void Start()
        {
            DIContainer.Resolve<GameController>().Bootstrap();
        }
    };
}
