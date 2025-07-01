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
                var instigator_entity = request.instigatorEntity;
                
                var instigator_marker = instigator_entity.GetComponent<CharacterMarker>();
                var instigator_team = instigator_marker.metaEntity.GetComponent<Team>().value;
                
                var instigator_tr = instigator_entity.GetComponent<ObjectRef<Transform>>().Target;
                var instigator_pos = instigator_tr.position;
                
                float nearest_distance = float.MaxValue;

                var instigator_target = instigator_marker.metaEntity.GetComponent<CharacterTarget>();
                instigator_target.entity = null;
                
                foreach (var other_character_entity in m_charactersFilter)
                {
                    if (instigator_entity == other_character_entity) continue;

                    var other_character_marker = other_character_entity.GetComponent<CharacterMarker>();
                    var other_character_team = other_character_marker.metaEntity.GetComponent<Team>().value;
                    
                    if (instigator_team == other_character_team) continue;
                    
                    var other_character_tr = other_character_entity.GetComponent<ObjectRef<Transform>>().Target;

                    var direction = (instigator_pos - other_character_tr.position);
                    float distance = direction.sqrMagnitude;
                    
                    if (distance < nearest_distance)
                    {
                        nearest_distance = distance;
                        instigator_target.entity = other_character_entity;
                    }
                }

                request.State = (instigator_target.entity != null) 
                    ? EPromiseState.Fulfilled 
                    : EPromiseState.Rejected;
                
                Debug.Log($"FindNearestEnemyRequestHandleSystem.Response = {request.State}");
            }
        }
    };
}
