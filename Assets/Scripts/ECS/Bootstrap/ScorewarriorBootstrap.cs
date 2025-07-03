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
                .BindSystem<WeaponFireSystem>()
                .BindSystem<WeaponReloadSystem>()
                .BindSystem<ProjectileSpawnSystem>()
                .BindSystem<ProjectileMoveSystem>()
                .BindSystem<ProjectileHitSystem>()
                
                .BindSystem<TakeDamageSystem>()
                
                .BindSystem<FindNearestEnemyRequestHandleSystem>()
                ;
            
            pipeline
                .BindSystem<CharacterAddModifiersSystem>()
                .BindSystem<CharacterAddStatsSystem>()
                .BindSystem<CharacterAddMetaSystem>()
                .BindSystem<WeaponAddModifiersSystem>()
                .BindSystem<WeaponAddStatsSystem>()
                .BindSystem<WeaponAddMetaSystem>()
                .BindSystem<ProjectileAddStatsSystem>()
                .BindSystem<ProjectileAddMetaSystem>()
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
                .BindEvent<CharacterDied>()
                .BindEvent<WeaponFireCommand>()
                .BindEvent<WeaponReloadCommand>()
                .BindEvent<ProjectileSpawnCommand>()
                .BindEvent<TakeDamageCommand>()
                ;
        }

        void Start()
        {
            DIContainer.Resolve<GameController>().Bootstrap();
        }
    };
}
