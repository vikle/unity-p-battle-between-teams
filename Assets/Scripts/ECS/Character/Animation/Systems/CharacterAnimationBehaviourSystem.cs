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
    public sealed class CharacterAnimationBehaviourSystem : IUpdateSystem
    {
        readonly Filter m_charactersFilter;
        readonly GameController m_gameController;
        
        [Preserve]public CharacterAnimationBehaviourSystem(Pipeline pipeline)
        {
            m_charactersFilter = pipeline.Query
                                         .With<CharacterMarker>()
                                         .With<CharacterAnimationMarker>()
                                         .Build();
            
            DIContainer.Resolve(out m_gameController);
        }

        public void OnUpdate(Pipeline pipeline)
        {
            if (m_gameController.GameState != EGameState.Started) return;
            
            foreach (var character in m_charactersFilter)
            {
                var chr_meta = character.GetComponent<CharacterMarker>().meta;
                var anim_meta = character.GetComponent<CharacterAnimationMarker>().meta;

                var state = chr_meta.GetComponent<CharacterState>().value;
                var hash = anim_meta.GetComponent<CharacterAnimationHash>();
                var validation = anim_meta.GetComponent<CharacterAnimationHashValidation>();
                var animator = character.GetComponent<ObjectRef<Animator>>().Target;

                if (validation.aiming)
                {
                    animator.SetBool(hash.aiming, state is ECharacterState.Aiming 
                                         or ECharacterState.TryShooting 
                                         or ECharacterState.ShootFire 
                                         or ECharacterState.Reloading);
                }
                
                if (validation.reloading)
                {
                    animator.SetBool(hash.reloading, state is ECharacterState.Reloading);
                }
                
                switch (state)
                {
                    case ECharacterState.ShootFire:
                        if (validation.shoot)
                        {
                            animator.SetTrigger(hash.shoot);
                        }
                        break;
                    case ECharacterState.Reloading: 
                        if (validation.reloadTime)
                        {
                            var weapon = chr_meta.GetComponent<CharacterWeapon>().entity;
                            var weapon_stats = weapon.GetComponent<WeaponMarker>().stats;
                            float reload_time = weapon_stats.GetComponent<ReloadTime>().value;
                            float reload_animation_length = anim_meta.GetComponent<ReloadTime>().value;
                            animator.SetFloat(hash.reloadTime, reload_time / reload_animation_length);
                        }
                        break;
                    case ECharacterState.Die: 
                        if (validation.die)
                        {
                            animator.SetTrigger(hash.die);
                        }
                        break;
                    default: break;
                }
            }
        }
    };
}
