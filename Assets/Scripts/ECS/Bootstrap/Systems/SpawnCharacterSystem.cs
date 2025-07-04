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
    public sealed class SpawnCharacterSystem : IUpdateSystem
    {
        readonly Filter m_filter;
        readonly GameController m_gameController;
        
        [Preserve]public SpawnCharacterSystem(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<SpawnCharacterRequest>().Build();
            DIContainer.Resolve(out m_gameController);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_filter.IsEmpty) return;

            foreach (var cmd_entity in m_filter)
            {
                var request = cmd_entity.GetComponent<SpawnCharacterRequest>();
                request.State = EPromiseState.Fulfilled;
                
                var prefab_clone = GameObjectPool.Instantiate(request.prefab, request.position);
            
                var actor = prefab_clone.GetComponent<EntityActor>();
                actor.InitEntity();

                var entity = actor.EntityRef;
                var marker = entity.GetComponent<CharacterMarker>();

                var prefab = entity.GetComponent<ObjectRef<CharacterPrefab>>().Target;
                
                var meta = marker.meta;
                meta.GetComponent<Team>().value = request.team;
                meta.GetComponent<Sector>().value = request.sector;
                meta.GetComponent<CharacterHitBox>().transform = prefab._hitBox;
                
                pipeline.Trigger<CharacterSpawned>().character = entity;
                
                m_gameController.FreeWaiter();
                
                break;
            }
        }
    };
}
