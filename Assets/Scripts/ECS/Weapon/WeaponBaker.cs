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
        public override void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.AddComponent<WeaponMarker>();
            marker.statsEntity = pipeline.CreateEntity();
            marker.modifiersEntity = pipeline.CreateEntity();
        }

        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.GetComponent<WeaponMarker>();
            marker.statsEntity.Destroy();
            marker.modifiersEntity.Destroy();
        }
    };
}
