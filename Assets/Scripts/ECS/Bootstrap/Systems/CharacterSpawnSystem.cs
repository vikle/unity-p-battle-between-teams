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
    public sealed class CharacterSpawnSystem : IUpdateSystem
    {
        readonly Filter m_filter;
        
        [Preserve]public CharacterSpawnSystem(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<CharacterSpawnTask>().Build();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_filter.IsEmpty) return;

            foreach (var spawn_task_entity in m_filter)
            {
                var spawn_task = spawn_task_entity.GetComponent<CharacterSpawnTask>();
                
                var prefab_clone = GameObjectPool.Instantiate(spawn_task.prefab, spawn_task.position);
            
                var actor = prefab_clone.GetComponent<EntityActor>();
                actor.InitEntity();

                var entity = actor.EntityRef;
                var marker = entity.GetComponent<CharacterMarker>();
                
                var meta_entity = marker.metaEntity;
                meta_entity.GetComponent<Team>().value = spawn_task.team;
                meta_entity.GetComponent<Sector>().value = spawn_task.sector;
                
                pipeline.Trigger<CharacterSpawned>().characterEntity = entity;
            }
        }
    };
}
