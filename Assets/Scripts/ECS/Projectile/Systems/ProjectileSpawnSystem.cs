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
    public sealed class ProjectileSpawnSystem : IUpdateSystem
    {
        readonly Filter m_filter;
        
        [Preserve]public ProjectileSpawnSystem(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<ProjectileSpawnCommand>().Build();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_filter.IsEmpty) return;

            foreach (var cmd_entity in m_filter)
            {
                var cmd = cmd_entity.GetComponent<ProjectileSpawnCommand>();

                var prefab = GameObjectPool.Instantiate(cmd.prefab, cmd.position);
                
                var actor = prefab.GetComponent<EntityActor>();
                actor.InitEntity();

                var entity = actor.EntityRef;

                var marker = entity.GetComponent<ProjectileMarker>();
                var meta = marker.meta;

                var target = meta.GetComponent<ProjectileTarget>();
                
                target.entity = cmd.target;
                target.position = cmd.hitBoxPosition;
                target.distance = Vector3.Distance(cmd.position, target.position);
                target.hit = cmd.hit;

                var move_meta = meta.GetComponent<ProjectileMoveMeta>();

                move_meta.origin = cmd.position;
                move_meta.direction = Vector3.Normalize(target.position - cmd.position);
                move_meta.rayPosition = 0f;
                
                meta.GetComponent<Damage>().value = cmd.damage;
            }
        }
    };
}
