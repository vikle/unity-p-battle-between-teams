using UnityEngine.Scripting;
using UniversalEntities;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Scorewarrior.DebugGame
{
    using ECS;
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public sealed class DebugInspectorUpdateSystem : IUpdateSystem
    {
        readonly Filter m_gameStateChangedFilter;
        readonly Filter m_characterSpawnedFilter;
        readonly Filter m_characterStateChangedFilter;
        readonly Filter m_characterDamageTakenFilter;
        
        readonly DebugInspectorModel m_model;
        
        [Preserve]public DebugInspectorUpdateSystem(Pipeline pipeline)
        {
            m_gameStateChangedFilter = pipeline.Query.With<GameStateChanged>().Build();
            m_characterSpawnedFilter = pipeline.Query.With<CharacterSpawned>().Build();
            m_characterStateChangedFilter = pipeline.Query.With<CharacterStateChanged>().Build();
            m_characterDamageTakenFilter = pipeline.Query.With<CharacterDamageTaken>().Build();

            DIContainer.Resolve(out m_model);
        }
        
        public void OnUpdate(Pipeline pipeline)
        {
            GameStateChanged_Process();
            CharacterSpawned_Process();
            CharacterStateChanged_Process();
            CharacterDamageTaken_Process();
        }
        
        private void GameStateChanged_Process()
        {
            if (m_gameStateChangedFilter.IsEmpty) return;

            foreach (var evt_entity in m_gameStateChangedFilter)
            {
                var game_state = evt_entity.GetComponent<GameStateChanged>().value;
                m_model.onGameStateChanged?.Invoke(game_state);
            }
        }
        
        private void CharacterSpawned_Process()
        {
            if (m_characterSpawnedFilter.IsEmpty) return;
            
            foreach (var evt_entity in m_characterSpawnedFilter)
            {
                var character = evt_entity.GetComponent<CharacterSpawned>().character;
                m_model.onCharacterSpawned?.Invoke(character);
            }
        }
        
        private void CharacterStateChanged_Process()
        {
            if (m_characterStateChangedFilter.IsEmpty) return;
            
            foreach (var evt_entity in m_characterStateChangedFilter)
            {
                var character = evt_entity.GetComponent<CharacterStateChanged>().character;
                m_model.onCharacterStateChanged?.Invoke(character);
            }
        }
        
        private void CharacterDamageTaken_Process()
        {
            if (m_characterDamageTakenFilter.IsEmpty) return;
            
            foreach (var evt_entity in m_characterDamageTakenFilter)
            {
                var character = evt_entity.GetComponent<CharacterDamageTaken>().character;
                m_model.onCharacterDamageTaken?.Invoke(character);
            }
        }
    };
}