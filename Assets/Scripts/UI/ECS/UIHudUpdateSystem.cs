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
        readonly Filter m_characterDamageTakenFilter;
        readonly Filter m_characterDiedFilter;
        readonly UIHudModel m_model;
        
        [Preserve]public UIHudUpdateSystem(Pipeline pipeline)
        {
            m_characterSpawnedFilter = pipeline.Query.With<CharacterSpawned>().Build();
            m_characterDamageTakenFilter = pipeline.Query.With<CharacterDamageTaken>().Build();
            m_characterDiedFilter = pipeline.Query.With<CharacterDied>().Build();
            DIContainer.Resolve(out m_model);
        }

        public void OnUpdate(Pipeline pipeline)
        {
            SpawnCharacters_Process();
            TakeCharactersDamage_Process();
            CharacterDied_Process();
        }

        private void SpawnCharacters_Process()
        {
            if (m_characterSpawnedFilter.IsEmpty) return;

            foreach (var evt_entity in m_characterSpawnedFilter)
            {
                var character = evt_entity.GetComponent<CharacterSpawned>().character;
                m_model.Call_OnCharacterSpawned(character);
            }
        }
        
        private void TakeCharactersDamage_Process()
        {
            if (m_characterDamageTakenFilter.IsEmpty) return;
            
            foreach (var evt_entity in m_characterDamageTakenFilter)
            {
                var character = evt_entity.GetComponent<CharacterDamageTaken>().character;
                m_model.Call_OnCharacterDamageTaken(character);
            }
        }

        private void CharacterDied_Process()
        {
            if (m_characterDiedFilter.IsEmpty) return;
            
            foreach (var evt_entity in m_characterDiedFilter)
            {
                var character = evt_entity.GetComponent<CharacterDied>().character;
                m_model.Call_OnCharacterDie(character);
            }
        }
    };
}
