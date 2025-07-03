using UnityEngine.Scripting;
using UniversalEntities;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Scorewarrior.ECS
{
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public sealed class SpawnCharacterSystem : IUpdateSystem
    {
        readonly Filter m_filter;
        
        [Preserve]public SpawnCharacterSystem(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<SpawnCharacterCommand>().Build();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_filter.IsEmpty) return;

            foreach (var cmd_entity in m_filter)
            {
                var cmd = cmd_entity.GetComponent<SpawnCharacterCommand>();
                
                var prefab_clone = GameObjectPool.Instantiate(cmd.prefab, cmd.position);
            
                var actor = prefab_clone.GetComponent<EntityActor>();
                actor.InitEntity();

                var entity = actor.EntityRef;
                var marker = entity.GetComponent<CharacterMarker>();

                var prefab = entity.GetComponent<ObjectRef<CharacterPrefab>>().Target;
                
                var meta = marker.meta;
                meta.GetComponent<Team>().value = cmd.team;
                meta.GetComponent<Sector>().value = cmd.sector;
                meta.GetComponent<CharacterHitBox>().transform = prefab._hitBox;
                
                pipeline.Trigger<CharacterSpawned>().character = entity;
            }
        }
    };
}
