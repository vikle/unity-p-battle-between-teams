using UnityEngine.Scripting;
using UniversalEntities;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Scorewarrior.ECS
{
    using UI;
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public sealed class UIControllerUpdateSystem : IUpdateSystem
    {
        readonly Filter m_gameStateChangedFilter;
        readonly UIController m_uiController;
        
        [Preserve]public UIControllerUpdateSystem(Pipeline pipeline)
        {
            m_gameStateChangedFilter = pipeline.Query.With<GameStateChanged>().Build();
            DIContainer.TryGet(out m_uiController);
        }
        
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_gameStateChangedFilter.IsEmpty) return;

            foreach (var game_state_entity in m_gameStateChangedFilter)
            {
                var game_state = game_state_entity.GetComponent<GameStateChanged>().value;
                m_uiController.OnGameStateChanged(game_state);
            }
        }
    };
}
