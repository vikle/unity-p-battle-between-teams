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
    public sealed class ProjectileHitSystem : IUpdateSystem
    {
        readonly Filter m_filter;

        [Preserve]public ProjectileHitSystem(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<ProjectileMarker>() .Build();
        }
        
        public void OnUpdate(Pipeline pipeline)
        {
            foreach (var entity in m_filter)
            {
                var marker = entity.GetComponent<ProjectileMarker>();
                var meta = marker.meta;

                var target = meta.GetComponent<ProjectileTarget>();
                var move = meta.GetComponent<ProjectileMoveMeta>();

                if (move.rayPosition < target.distance)
                {
                    continue;
                }
                
                if (target.hit)
                {
                    var dmg_cmd = pipeline.Trigger<TakeDamageCommand>();
                    dmg_cmd.damage = meta.GetComponent<Damage>().value;
                    dmg_cmd.target = target.entity;
                }

                GameObjectPool.Return(entity);
            }
        }
    }
}
