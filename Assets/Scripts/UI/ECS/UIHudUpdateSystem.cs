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
    public sealed class UIHudUpdateSystem : IUpdateSystem
    {
        readonly Filter m_characterSpawnedFilter;
        
        readonly UIHud m_uiHud;
        
        [Preserve]public UIHudUpdateSystem(Pipeline pipeline)
        {
            m_characterSpawnedFilter = pipeline.Query.With<CharacterSpawned>().Build();
            
            DIContainer.TryGet(out m_uiHud);
        }

        public void OnUpdate(Pipeline pipeline)
        {
            SpawnCharacters_Process();
            TakeCharactersDamage_Process();
        }

        private void SpawnCharacters_Process()
        {
            if (m_characterSpawnedFilter.IsEmpty) return;

            foreach (var event_entity in m_characterSpawnedFilter)
            {
                
            }
        }
        
        private void TakeCharactersDamage_Process()
        {
            
        }
    };
}
