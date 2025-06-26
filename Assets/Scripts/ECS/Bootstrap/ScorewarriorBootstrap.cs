using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class ScorewarriorBootstrap : UniversalEntitiesBootstrap
    { 
        public override void OnBootstrap(Pipeline pipeline)
        {
            DIContainer.Register(pipeline);
            
            pipeline
                .BindSystem<CharacterAddStatsSystem>()
                .BindSystem<CharacterAddModifiersSystem>()
                .BindSystem<WeaponAddStatsSystem>()
                .BindSystem<WeaponAddModifiersSystem>()
                ; 

            
            
            
            pipeline
                .BindEvent<GameStateChanged>()
                .BindEvent<CharacterSpawned>()
                ;
        }
    };
}
