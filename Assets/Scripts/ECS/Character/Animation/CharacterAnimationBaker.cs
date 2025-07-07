using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    using Descriptors;
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterAnimationDescriptor))]
    public sealed class CharacterAnimationBaker : EntityActorBaker
    {
        public CharacterAnimationDescriptor descriptor;
        
#if UNITY_EDITOR
        void OnValidate()
        {
            if (descriptor == null) descriptor = GetComponent<CharacterAnimationDescriptor>();
        }
#endif
        public override void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.AddComponent<CharacterAnimationMarker>();
            marker.meta = pipeline.CreateEntity();
            
            entity.AddComponent<ObjectRef<Animator>>().Target = descriptor._animator;
            entity.AddComponent<ObjectRef<CharacterAnimationDescriptor>>().Target = descriptor;
        }

        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.GetComponent<CharacterAnimationMarker>();
            marker.meta.Destroy();
        }
    };
}
