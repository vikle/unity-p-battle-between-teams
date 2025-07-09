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
    public sealed class TakeDamageSystem : IUpdateSystem
    {
        readonly Filter m_filter;

        [Preserve]public TakeDamageSystem(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<TakeDamageCommand>().Build();
        }
        
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_filter.IsEmpty) return;

            foreach (var cmd_entity in m_filter)
            {
                var cmd = cmd_entity.GetComponent<TakeDamageCommand>();
                var target = cmd.target;

                var target_meta = target.GetComponent<CharacterMarker>().meta;
                
                ref var target_state = ref target_meta.GetComponent<CharacterState>().value;

                if (target_state == ECharacterState.Die)
                {
                    continue;
                }
                
                pipeline.Trigger<CharacterDamageTaken>().character = target;

                ref float target_armor = ref target_meta.GetComponent<Armor>().value;
                ref float target_health = ref target_meta.GetComponent<Health>().value;
                
                if (target_armor > 0f)
                {
                    target_armor -= cmd.damage;

                    if (target_armor < 0f)
                    {
                        target_health += target_armor;
                    }
                    
                    continue;
                } 
                
                target_health -= cmd.damage;
                
                if (target_health > 0f) continue;
                
                target_state = ECharacterState.Die;
                pipeline.Trigger<CharacterDied>().character = target;
                
                var state_ch_evt = pipeline.Trigger<CharacterStateChanged>();
                state_ch_evt.character = target;
                state_ch_evt.state = target_state;
            }
        }
    };
}
