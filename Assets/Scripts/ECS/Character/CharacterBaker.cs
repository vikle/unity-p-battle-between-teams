using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    using Test.Descriptors;
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterPrefab))]
    [RequireComponent(typeof(CharacterDescriptor))]
    [RequireComponent(typeof(CharacterDescriptorProvider))]
    public sealed class CharacterBaker : EntityActorBaker
    {
        public CharacterPrefab prefab;
        public CharacterDescriptor descriptor;
        public CharacterDescriptorProvider provider;

#if UNITY_EDITOR
        void OnValidate()
        {
            if (prefab == null) prefab = GetComponent<CharacterPrefab>();
            if (descriptor == null) descriptor = GetComponent<CharacterDescriptor>();
            if (provider == null) provider = GetComponent<CharacterDescriptorProvider>();
        }
#endif
        public override void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.AddComponent<CharacterMarker>();
            marker.modifiers = pipeline.CreateEntity();
            marker.stats = pipeline.CreateEntity();
            marker.meta = pipeline.CreateEntity();

            entity.AddComponent<ObjectRef<CharacterPrefab>>().Target = prefab;
            entity.AddComponent<ObjectRef<CharacterDescriptor>>().Target = descriptor;
            entity.AddComponent<ObjectRef<CharacterDescriptorProvider>>().Target = provider;
        }

        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.GetComponent<CharacterMarker>();
            marker.modifiers.Destroy();
            marker.stats.Destroy();
            marker.meta.Destroy();
        }
    };
}
