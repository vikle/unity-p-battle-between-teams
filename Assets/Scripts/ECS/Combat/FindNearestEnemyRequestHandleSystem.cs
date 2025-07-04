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
    public sealed class FindNearestEnemyRequestHandleSystem : IUpdateSystem
    {
        readonly Filter m_requestFilter;
        readonly Filter m_charactersFilter;

        [Preserve]public FindNearestEnemyRequestHandleSystem(Pipeline pipeline)
        {
            m_requestFilter = pipeline.Query.With<FindNearestEnemyRequest>().Build();
            m_charactersFilter = pipeline.Query.With<CharacterMarker>().Build();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_requestFilter.IsEmpty) return;

            foreach (var request_entity in m_requestFilter)
            {
                var request = request_entity.GetComponent<FindNearestEnemyRequest>();
                var instigator = request.instigator;
                
                var instigator_marker = instigator.GetComponent<CharacterMarker>();
                var instigator_team = instigator_marker.meta.GetComponent<Team>().value;
                
                var instigator_tr = instigator.GetComponent<ObjectRef<Transform>>().Target;
                var instigator_pos = instigator_tr.position;
                
                float nearest_distance = float.MaxValue;

                var instigator_target = instigator_marker.meta.GetComponent<CharacterTarget>();
                instigator_target.entity = null;
                
                foreach (var other_character in m_charactersFilter)
                {
                    if (instigator == other_character) continue;

                    var other_marker = other_character.GetComponent<CharacterMarker>();
                    var other_meta = other_marker.meta;

                    if (other_meta.GetComponent<CharacterState>().value == ECharacterState.Die) continue;
                    if (instigator_team == other_meta.GetComponent<Team>().value) continue;
                    
                    var other_tr = other_character.GetComponent<ObjectRef<Transform>>().Target;
                    var direction = (instigator_pos - other_tr.position);
                    float distance = direction.sqrMagnitude;
                    
                    if (distance > nearest_distance) continue;
                    
                    nearest_distance = distance;
                    instigator_target.entity = other_character;
                }

                request.State = (instigator_target.entity != null) 
                    ? EPromiseState.Fulfilled 
                    : EPromiseState.Rejected;
                
                Debug.Log($"FindNearestEnemyRequestHandleSystem.Response = {request.State}");
            }
        }
    };
}
