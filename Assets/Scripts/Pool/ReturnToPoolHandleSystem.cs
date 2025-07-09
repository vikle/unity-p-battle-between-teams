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
    public sealed class ReturnToPoolHandleSystem : ICollectSystem
    {
        readonly Filter m_filter;

        [Preserve]public ReturnToPoolHandleSystem(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<ReturnToPoolCommand>().Build();
        }
        
        public void OnCollect(Pipeline pipeline)
        {
            if (m_filter.IsEmpty) return;

            foreach (var entity in m_filter)
            {
                var cmd = entity.GetComponent<ReturnToPoolCommand>();
                GameObjectPool.Return(cmd.entity);
            }
        }
    };
}
