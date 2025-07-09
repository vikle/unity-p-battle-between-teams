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
    public sealed class WeaponReloadSystem : IUpdateSystem
    {
        readonly Filter m_filter;
        
        [Preserve]public WeaponReloadSystem(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<WeaponReloadCommand>().Build();
        }
        
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_filter.IsEmpty) return;

            foreach (var cmd_entity in m_filter)
            {
                var cmd = cmd_entity.GetComponent<WeaponReloadCommand>();
                var marker = cmd.weapon.GetComponent<WeaponMarker>();
                uint clip_size = marker.stats.GetComponent<ClipSize>().value;
                marker.meta.GetComponent<ClipSize>().value = clip_size;
            }
        }
    };
}
