using System;
using System.Linq;
using UnityEngine;
using UniversalEntities;
using Random = UnityEngine.Random;

namespace Scorewarrior.ECS
{
    using Test.Descriptors;
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterDescriptor))]
    public sealed class CharacterBaker : EntityActorBaker
    {
        public override void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.AddComponent<CharacterMarker>();
            marker.StatsEntity = pipeline.CreateEntity();
            marker.ModifiersEntity = pipeline.CreateEntity();

            AddStats(marker.StatsEntity);
            AddModifiers(marker.ModifiersEntity);
        }

        private void AddStats(Entity entity)
        {
            var descriptor = GetComponent<CharacterDescriptor>();
            
            entity.AddComponent<Accuracy>().Value = descriptor.Accuracy;
            entity.AddComponent<Dexterity>().Value = descriptor.Dexterity;
            entity.AddComponent<Health>().Value = descriptor.MaxHealth;
            entity.AddComponent<Armor>().Value = descriptor.MaxArmor;
            entity.AddComponent<AimTime>().Value = descriptor.AimTime;
        }

        private void AddModifiers(Entity entity)
        {
            entity.AddComponent<Accuracy>().Value = 1f;
            entity.AddComponent<Dexterity>().Value = 1f;
            entity.AddComponent<Health>().Value = 1f;
            entity.AddComponent<Armor>().Value = 1f;
            entity.AddComponent<AimTime>().Value = 1f;
            
            var provider = GetComponent<CharacterDescriptorProvider>();

            var available_modifiers = Enum.GetValues(typeof(EDescriptor)).Cast<EDescriptor>().ToList();
            
            for (int i = 0, i_max = provider.modifiersCount; i < i_max; i++)
            {
                int modifier_index = Random.Range(0, available_modifiers.Count);
                var modifier = available_modifiers[modifier_index];
                float value = Random.Range(provider.modificationRange.x, provider.modificationRange.y);
                
                switch (modifier)
                {
                    case EDescriptor.Accuracy: entity.GetComponent<Accuracy>().Value = value; break;
                    case EDescriptor.Dexterity: entity.GetComponent<Dexterity>().Value = value; break;
                    case EDescriptor.MaxHealth: entity.GetComponent<Health>().Value = value; break;
                    case EDescriptor.MaxArmor: entity.GetComponent<Armor>().Value = value; break;
                    case EDescriptor.AimTime: entity.GetComponent<AimTime>().Value = value; break;
                    default: break;
                }
                
                available_modifiers.RemoveAt(modifier_index);
            }
        }

        public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor)
        {
            var marker = entity.GetComponent<CharacterMarker>();
            marker.StatsEntity.Destroy();
            marker.ModifiersEntity.Destroy();
        }
    };
}
