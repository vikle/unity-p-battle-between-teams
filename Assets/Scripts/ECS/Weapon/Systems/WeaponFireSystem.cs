using Scorewarrior.Test.Views;
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
    public sealed class WeaponFireSystem : IUpdateSystem
    {
        readonly Filter m_filter;
        
        [Preserve]public WeaponFireSystem(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<WeaponFireCommand>().Build();
        }
        
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_filter.IsEmpty) return;

            foreach (var cmd_entity in m_filter)
            {
                var cmd = cmd_entity.GetComponent<WeaponFireCommand>();

                var weapon_marker = cmd.weapon.GetComponent<WeaponMarker>();
                var owner_marker = cmd.owner.GetComponent<CharacterMarker>();
                var target_marker = cmd.target.GetComponent<CharacterMarker>();

                var weapon_prefab = cmd.weapon.GetComponent<ObjectRef<WeaponPrefab>>().Target;
                
                float hit_chance = Random.value;

                bool hit = (hit_chance <= owner_marker.stats.GetComponent<Accuracy>().value
                         && hit_chance <= weapon_marker.stats.GetComponent<Accuracy>().value
                         && hit_chance >= target_marker.stats.GetComponent<Dexterity>().value);

                var target_hit_box = target_marker.meta.GetComponent<CharacterHitBox>();
                
                var proj_spawn_cmd = pipeline.Trigger<ProjectileSpawnCommand>();
                proj_spawn_cmd.prefab = weapon_prefab.projectilePrefab;
                proj_spawn_cmd.position = weapon_prefab._barrelTransform.position;
                proj_spawn_cmd.target = cmd.target;
                proj_spawn_cmd.damage = weapon_marker.stats.GetComponent<Damage>().value;
                proj_spawn_cmd.hit = hit;
                proj_spawn_cmd.hitBoxPosition = target_hit_box.transform.position;

                ref float last_fire_time = ref weapon_marker.meta.GetComponent<FireRate>().value;
                float fire_rate = weapon_marker.stats.GetComponent<FireRate>().value;
                last_fire_time = (TimeData.Time + 1f / fire_rate);

                weapon_marker.meta.GetComponent<ClipSize>().value--;
            }
        }
    };
}
