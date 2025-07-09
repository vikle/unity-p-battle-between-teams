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
    public sealed class CharacterDiedHandleSystem : IUpdateSystem
    {
        readonly Filter m_characterDiedFilter;
        readonly Filter m_charactersFilter;
        readonly GameController m_gameController;
        
        [Preserve]public CharacterDiedHandleSystem(Pipeline pipeline)
        {
            m_characterDiedFilter = pipeline.Query.With<CharacterDied>().Build();
            m_charactersFilter = pipeline.Query.With<CharacterMarker>().Build();
            DIContainer.Resolve(out m_gameController);
        }
        
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_characterDiedFilter.IsEmpty) return;

            foreach (var evt_entity in m_characterDiedFilter)
            {
                var character = evt_entity.GetComponent<CharacterDied>().character;
                var marker = character.GetComponent<CharacterMarker>();
                var team = marker.meta.GetComponent<Team>().value;
                if (TeamIsDied(team)) m_gameController.FinishGame();
            }
            
            foreach (var evt_entity in m_characterDiedFilter)
            {
                var character = evt_entity.GetComponent<CharacterDied>().character;
                RemoveDiedTargets(pipeline, character);
            }
        }

        private bool TeamIsDied(ETeam diedCharacterTeam)
        {
            foreach (var character in m_charactersFilter)
            {
                var marker = character.GetComponent<CharacterMarker>();
                var team = marker.meta.GetComponent<Team>().value;
                if (team != diedCharacterTeam) continue;
                float health = marker.meta.GetComponent<Health>().value;
                if (health > 0f) return false;
            }
            
            return true;
        }

        private void RemoveDiedTargets(Pipeline pipeline, Entity diedCharacter)
        {
            foreach (var character in m_charactersFilter)
            {
                if (character == diedCharacter) continue;
                    
                var marker = character.GetComponent<CharacterMarker>();
                var target = marker.meta.GetComponent<CharacterTarget>();
                if (target.entity != diedCharacter) continue;
                target.entity = null;
                pipeline.Then<FindNearestEnemyRequest>().instigator = character;
            }
        }
    };
}
