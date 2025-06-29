using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    using Test.Descriptors;
    using Test.Views;
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(WeaponPrefab))]
    [RequireComponent(typeof(WeaponDescriptor))]
    [RequireComponent(typeof(WeaponDescriptorProvider))]
    public sealed class WeaponBaker : EntityActorBaker
    {
        public WeaponPrefab prefab;
        public WeaponDescriptor descriptor;
        public WeaponDescriptorProvider provider;

#if UNITY_EDITOR
        void OnValidate()
        {
            if (prefab == null) prefab = GetComponent<WeaponPrefab>();
            if (descriptor == null) descriptor = GetComponent<WeaponDescriptor>();
            if (provider == null) provider = GetComponent<WeaponDescriptorProvider>();
        }
#endif
        public override void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.AddComponent<WeaponMarker>();
            marker.statsEntity = pipeline.CreateEntity();
            marker.modifiersEntity = pipeline.CreateEntity();
            marker.metaEntity = pipeline.CreateEntity();
            
            entity.AddComponent<ObjectRef<WeaponPrefab>>().Target = prefab;
            entity.AddComponent<ObjectRef<WeaponDescriptor>>().Target = descriptor;
            entity.AddComponent<ObjectRef<WeaponDescriptorProvider>>().Target = provider;
        }

        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.GetComponent<WeaponMarker>();
            marker.statsEntity.Destroy();
            marker.modifiersEntity.Destroy();
            marker.metaEntity.Destroy();
        }
    };
}
