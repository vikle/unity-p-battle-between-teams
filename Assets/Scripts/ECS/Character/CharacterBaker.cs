using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    using Test.Descriptors;
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterDescriptor))]
    [RequireComponent(typeof(CharacterDescriptorProvider))]
    public sealed class CharacterBaker : EntityActorBaker
    {
        public override void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.AddComponent<CharacterMarker>();
            marker.statsEntity = pipeline.CreateEntity();
            marker.modifiersEntity = pipeline.CreateEntity();
            marker.metaEntity = pipeline.CreateEntity();
        }

        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.GetComponent<CharacterMarker>();
            marker.statsEntity.Destroy();
            marker.modifiersEntity.Destroy();
            marker.metaEntity.Destroy();
        }
    };
}
