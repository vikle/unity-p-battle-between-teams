using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class ScorewarriorBootstrap : UniversalEntitiesBootstrap
    { 
        public override void OnBootstrap(Pipeline pipeline)
        {
            DIContainer.Register(pipeline);
            
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
                .BindSystem<CharacterAnimationAddMetaSystem>()
                ; 
            
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
                .BindSystem<CharacterDiedHandleSystem>()
                .BindSystem<FindNearestEnemyRequestHandleSystem>()
                ;
            
            pipeline
                .BindSystem<CharacterAnimationBehaviourSystem>()
                ; 
            
            pipeline
                .BindSystem<UIControllerUpdateSystem>()
                .BindSystem<UIHudUpdateSystem>()
                ;
            
            pipeline
                .BindSystem<DebugGame.DebugInspectorUpdateSystem>()
                ; 
            
            BindCollect(pipeline);
            
            BindPromises(pipeline);
            BindEvents(pipeline);
        }

        private static void BindCollect(Pipeline pipeline)
        {
            pipeline
                .BindSystem<ReturnToPoolHandleSystem>()
                ; 
        }

        private static void BindPromises(Pipeline pipeline)
        {
            pipeline
                .BindPromise<FindNearestEnemyRequest>()
                .BindPromise<SpawnCharacterRequest>()
                ;
        }
        
        private static void BindEvents(Pipeline pipeline)
        {
            pipeline
                .BindEvent<GameStateChanged>()
                .BindEvent<CharacterSpawned>()
                .BindEvent<CharacterStateChanged>()
                .BindEvent<CharacterDied>()
                .BindEvent<WeaponFireCommand>()
                .BindEvent<WeaponReloadCommand>()
                .BindEvent<ProjectileSpawnCommand>()
                .BindEvent<TakeDamageCommand>()
                .BindEvent<CharacterDamageTaken>()
                ;

            pipeline
                .BindEvent<ReturnToPoolCommand>()
                ;
        }
        
        void Start()
        {
            DIContainer.Resolve<GameController>().Bootstrap();
        }
    };
}
