using System.Collections.Generic;
using UnityEngine;
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
    public sealed class GameStateStartingSystem : IUpdateSystem
    {
        readonly Filter m_gameStateChangedFilter;
        
        readonly SpawnPointSet m_spawnPointSet;
        readonly CharacterPrefabSet m_characterPrefabSet;
        
        [Preserve]public GameStateStartingSystem(Pipeline pipeline)
        {
            m_gameStateChangedFilter = pipeline.Query.With<GameStateChanged>().Build();
            
            DIContainer.TryGet(out m_spawnPointSet);
            DIContainer.TryGet(out m_characterPrefabSet);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_gameStateChangedFilter.IsEmpty) return;

            foreach (var entity in m_gameStateChangedFilter)
            {
                var game_state = entity.GetComponent<GameStateChanged>().value;
                if (game_state != EGameState.Starting) continue;

                var available_prefabs = new List<GameObject>(m_characterPrefabSet.characters);
            
                foreach (var spawn_point in m_spawnPointSet.spawnPoints)
                {
                    if (available_prefabs.Count == 0)
                    {
                        Debug.LogWarning($"Not found available character prefab for '{spawn_point.name}'");
                        continue;   
                    }
                    
                    int random_index = Random.Range(0, available_prefabs.Count);

                    var spawn_task = pipeline.Trigger<SpawnCharacterCommand>();
                    spawn_task.prefab = available_prefabs[random_index];
                    spawn_task.position = spawn_point.transform.position;
                    spawn_task.team = spawn_point.Team;
                    spawn_task.sector = spawn_point.Sector;
                    
                    available_prefabs.RemoveAt(random_index);
                }
                
                break;
            }
        }
    };
}
