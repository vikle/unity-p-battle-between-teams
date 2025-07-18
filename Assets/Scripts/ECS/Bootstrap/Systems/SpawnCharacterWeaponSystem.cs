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
    public sealed class SpawnCharacterWeaponSystem : IUpdateSystem
    {
        readonly Filter m_filter;

        [Preserve]public SpawnCharacterWeaponSystem(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<CharacterSpawned>().Build();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_filter.IsEmpty) return;

            foreach (var evt_entity in m_filter)
            {
                var character = evt_entity.GetComponent<CharacterSpawned>().character;
                var character_meta = character.GetComponent<CharacterMarker>().meta;
                var character_prefab = character.GetComponent<ObjectRef<CharacterPrefab>>().Target;
                
                var prefab = GameObjectPool.Instantiate(character_prefab._weaponPrefab);
                
                var transform = prefab.transform;
                transform.SetParent(character_prefab._weaponSlot);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                
                var actor = prefab.GetComponent<EntityActor>();
                actor.InitEntity();
                
                character_meta.GetComponent<CharacterWeapon>().entity = actor.EntityRef;
            }
        }
    };
}
