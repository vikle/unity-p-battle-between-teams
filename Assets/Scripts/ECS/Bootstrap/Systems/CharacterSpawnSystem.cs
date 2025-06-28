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

                var character_entity = actor.EntityRef;

                var meta_entity = character_entity.GetComponent<CharacterMarker>().metaEntity;
                meta_entity.AddComponent<Team>().value = spawn_task.team;
                meta_entity.AddComponent<Sector>().value = spawn_task.sector;
                meta_entity.AddComponent<CharacterState>().value = ECharacterState.Idle;
                meta_entity.AddComponent<CharacterTarget>().value = null;
                
                pipeline.Trigger<CharacterSpawned>().characterEntity = character_entity;
            }
        }
    };
}
