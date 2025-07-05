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
    public sealed class CharacterStateBehaviourSystem : IUpdateSystem
    {
        readonly Filter m_charactersFilter;

        [Preserve]public CharacterStateBehaviourSystem(Pipeline pipeline)
        {
            m_charactersFilter = pipeline.Query.With<CharacterMarker>().Build();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate(Pipeline pipeline)
        {
            foreach (var character in m_charactersFilter)
            {
                var marker = character.GetComponent<CharacterMarker>();
                var meta = marker.meta;
                var state_comp = meta.GetComponent<CharacterState>();
                ref var state_value = ref state_comp.value;

                if (state_value == ECharacterState.Die) continue;
                
                ref var target = ref meta.GetComponent<CharacterTarget>().entity;
                ref float next_aim_time = ref meta.GetComponent<AimTime>().value;

                bool target_is_valid = (target != null);

                if (target_is_valid)
                {
                    var target_meta = target.GetComponent<CharacterMarker>().meta;
                    var target_state = target_meta.GetComponent<CharacterState>().value;
                    target_is_valid = (target_state != ECharacterState.Die);
                }

                var weapon = meta.GetComponent<CharacterWeapon>().entity;

                switch (state_value)
                {
                    case ECharacterState.Idle:
                        if (!target_is_valid)
                        {
                            pipeline.Then<FindNearestEnemyRequest>().instigator = character;
                        }
                        else
                        {
                            float aim_time = marker.stats.GetComponent<AimTime>().value;
                            next_aim_time = (TimeData.Time + aim_time);
                            SwitchState(ECharacterState.Aiming, ref state_value, character, pipeline);
                            var character_transform = character.GetComponent<ObjectRef<Transform>>().Target;
                            var character_target_transform = target.GetComponent<ObjectRef<Transform>>().Target;
                            character_transform.LookAt(character_target_transform.position);
                        }
                        break;
                    
                    case ECharacterState.Aiming: 
                        if (target_is_valid)
                        {
                            if (next_aim_time < TimeData.Time)
                            {
                                SwitchState(ECharacterState.TryShooting, ref state_value, character, pipeline);
                            }
                        }
                        else
                        {
                            SwitchState(ECharacterState.Idle, ref state_value, character, pipeline);
                        }
                        break;
                    
                    case ECharacterState.TryShooting:
                    case ECharacterState.ShootFire:
                        if (target_is_valid)
                        {
                            var weapon_marker = weapon.GetComponent<WeaponMarker>();
                            var weapon_meta = weapon_marker.meta;
                            uint ammo = weapon_meta.GetComponent<ClipSize>().value;

                            if (ammo > 0u)
                            {
                                float last_fire_time = weapon_meta.GetComponent<FireRate>().value;
                                
                                if (last_fire_time < TimeData.Time)
                                {
                                    var cmd = pipeline.Trigger<WeaponFireCommand>();
                                    cmd.weapon = weapon;
                                    cmd.owner = character;
                                    cmd.target = target;
                                    
                                    SwitchState(ECharacterState.ShootFire, ref state_value, character, pipeline);
                                }
                                else
                                {
                                    SwitchState(ECharacterState.TryShooting, ref state_value, character, pipeline);
                                }
                            }
                            else
                            {
                                SwitchState(ECharacterState.Reloading, ref state_value, character, pipeline);
                                next_aim_time = weapon_marker.stats.GetComponent<ReloadTime>().value;
                            }
                        }
                        else
                        {
                            SwitchState(ECharacterState.Idle, ref state_value, character, pipeline);
                        }
                        break;
                    
                    case ECharacterState.Reloading:
                        if (next_aim_time < TimeData.Time)
                        {
                            var next_state = target_is_valid 
                                ? ECharacterState.TryShooting 
                                : ECharacterState.Idle;
                            
                            SwitchState(next_state, ref state_value, character, pipeline);
                            
                            var cmd = pipeline.Trigger<WeaponReloadCommand>();
                            cmd.weapon = weapon;
                        }
                        break;
                    
                    default: break;
                }
            }
        }
        
        private static void SwitchState(ECharacterState newState, ref ECharacterState currentState, Entity characterEntity, Pipeline pipeline)
        {
            if (currentState == newState) return;
            
            currentState = newState;
            
            var evt = pipeline.Trigger<CharacterStateChanged>();
            evt.character = characterEntity;
            evt.state = newState;

            Debug.Log($"CharacterStateBehaviourSystem.Trigger<CharacterStateChanged> = {newState}");
        }
    };
}
