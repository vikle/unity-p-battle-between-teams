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
                var meta = marker.metaEntity;
                var state_comp = meta.GetComponent<CharacterState>();
                var state_value = state_comp.value;

                if (state_value == ECharacterState.Die) continue;
                
                ref var character_target = ref meta.GetComponent<CharacterTarget>().entity;
                ref float next_aim_time = ref meta.GetComponent<AimTime>().value;

                bool target_is_valid = (character_target != null);

                if (target_is_valid && state_value != ECharacterState.Idle)
                {
                    var target_meta_entity = character_target.GetComponent<CharacterMarker>().metaEntity;
                    var target_state = target_meta_entity.GetComponent<CharacterState>().value;
                    target_is_valid = (target_state != ECharacterState.Die);
                }

                var weapon = meta.GetComponent<CharacterWeapon>().entity;

                switch (state_value)
                {
                    case ECharacterState.Idle:
                        if (character_target == null)
                        {
                            pipeline.Then<FindNearestEnemyRequest>().instigatorEntity = character;
                            Debug.Log($"CharacterStateBehaviourSystem.Then<FindNearestEnemyRequest>");
                        }
                        else
                        {
                            float aim_time = marker.statsEntity.GetComponent<AimTime>().value;
                            next_aim_time = (TimeData.Time + aim_time);
                            SwitchState(ECharacterState.Aiming, state_comp, character, pipeline);
                            var character_transform = character.GetComponent<ObjectRef<Transform>>().Target;
                            var character_target_transform = character_target.GetComponent<ObjectRef<Transform>>().Target;
                            character_transform.LookAt(character_target_transform.position);
                        }
                        break;
                    
                    case ECharacterState.Aiming: 
                        if (target_is_valid)
                        {
                            if (next_aim_time <= TimeData.Time)
                            {
                                SwitchState(ECharacterState.TryShooting, state_comp, character, pipeline);
                            }
                        }
                        else
                        {
                            SwitchState(ECharacterState.Idle, state_comp, character, pipeline);
                        }
                        break;
                    
                    case ECharacterState.TryShooting:
                    case ECharacterState.ShootFire:
                        if (target_is_valid)
                        {
                            var weapon_meta = weapon.GetComponent<WeaponMarker>().metaEntity;
                            uint ammo = weapon_meta.GetComponent<ClipSize>().value;

                            if (ammo > 0u)
                            {
                                float last_fire_time = weapon_meta.GetComponent<FireRate>().value;
                                
                                if (last_fire_time > TimeData.Time)
                                {
                                    var cmd = pipeline.Trigger<WeaponFireCommand>();
                                    cmd.weapon = weapon;
                                    cmd.owner = character;
                                    
                                    SwitchState(ECharacterState.ShootFire, state_comp, character, pipeline);
                                }
                                else
                                {
                                    SwitchState(ECharacterState.TryShooting, state_comp, character, pipeline);
                                }
                            }
                            else
                            {
                                SwitchState(ECharacterState.Reloading, state_comp, character, pipeline);
                                next_aim_time = weapon_meta.GetComponent<ReloadTime>().value;
                            }
                        }
                        else
                        {
                            SwitchState(ECharacterState.Idle, state_comp, character, pipeline);
                        }
                        break;
                    
                    case ECharacterState.Reloading:
                        if (next_aim_time <= TimeData.Time)
                        {
                            var next_state = target_is_valid 
                                ? ECharacterState.TryShooting 
                                : ECharacterState.Idle;
                            
                            SwitchState(next_state, state_comp, character, pipeline);
                            
                            var cmd = pipeline.Trigger<WeaponReloadCommand>();
                            cmd.weapon = weapon;
                            cmd.owner = character;
                        }
                        break;
                    
                    default: break;
                }
            }
        }
        
        private static void SwitchState(ECharacterState newState, CharacterState stateComp, Entity characterEntity, Pipeline pipeline)
        {
            var evt = pipeline.Trigger<CharacterStateChanged>();
            evt.characterEntity = characterEntity;
            evt.newState = newState;
            stateComp.value = newState;
            
            Debug.Log($"CharacterStateBehaviourSystem.Trigger<CharacterStateChanged> = {newState}");
        }
    };
}
