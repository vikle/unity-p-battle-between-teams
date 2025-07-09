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
    public sealed class ProjectileMoveSystem : IUpdateSystem
    {
        readonly Filter m_filter;
        readonly GameController m_gameController;

        [Preserve]public ProjectileMoveSystem(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<ProjectileMarker>() .Build();
            DIContainer.Resolve(out m_gameController);
        }
        
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_gameController.GameState != EGameState.Started) return;
            
            foreach (var entity in m_filter)
            {
                var marker = entity.GetComponent<ProjectileMarker>();
                var move = marker.meta.GetComponent<ProjectileMoveMeta>();

                float move_speed = marker.stats.GetComponent<Speed>().value;
                move.rayPosition += (TimeData.DeltaTime * move_speed);

                var transform = entity.GetComponent<ObjectRef<Transform>>().Target;
                transform.position = (move.origin + move.rayPosition * move.direction);
            }
        }
    };
}
