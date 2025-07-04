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
    public sealed class GameStateInitializeSystem : IUpdateSystem
    {
        readonly Filter m_gameStateChangedFilter;
        readonly Filter m_charactersFilter;
        
        [Preserve]public GameStateInitializeSystem(Pipeline pipeline)
        {
            m_gameStateChangedFilter = pipeline.Query.With<GameStateChanged>().Build();
            m_charactersFilter = pipeline.Query.With<CharacterMarker>().Build();
        }
        
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_gameStateChangedFilter.IsEmpty) return;

            foreach (var game_state_entity in m_gameStateChangedFilter)
            {
                var game_state = game_state_entity.GetComponent<GameStateChanged>().value;
                if (game_state != EGameState.Initiated) continue;

                foreach (var character_entity in m_charactersFilter)
                {
                    GameObjectPool.Return(character_entity);
                }
                
                break;
            }
        }
    };
}
