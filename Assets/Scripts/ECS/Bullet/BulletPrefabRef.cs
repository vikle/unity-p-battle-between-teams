using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    using Test.Views;
    
    public sealed class BulletPrefabRef : IComponent
    {
        public BulletPrefab originalObject;
        public Transform transform;
    };
}
