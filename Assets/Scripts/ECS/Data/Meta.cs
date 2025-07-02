using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class Team : IComponent
    {
        public ETeam value;
    };    
    
    public sealed class Sector : IComponent
    {
        public uint value;
    };  
    
    public sealed class CharacterState : IComponent
    {
        public ECharacterState value;
    };

    public sealed class CharacterTarget : IComponent
    {
        public Entity entity;
    };

    public sealed class CharacterWeapon : IComponent
    {
        public Entity entity;
    };
    
    public sealed class CharacterHitBox : IComponent
    {
        public Transform transform;
    };
    
    public sealed class ProjectileTarget : IComponent
    {
        public Entity entity;
    };
    
    public sealed class ProjectileMoveMeta : IComponent
    {
        public Vector3 startedPosition;
        public Vector3 moveDirection;
        public float totalDistance;
        public float currentDistance;
    };
}
