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
    
    public sealed class CharacterAnimationHash : IComponent
    {
        public int aiming;
        public int reloading;
        public int shoot;
        public int reloadTime;
        public int die;
    };
    
    public sealed class CharacterAnimationHashValidation : IComponent
    {
        public bool aiming;
        public bool reloading;
        public bool shoot;
        public bool reloadTime;
        public bool die;
    };
    
    public sealed class ProjectileTarget : IComponent
    {
        public Entity entity;
        public Vector3 position;
        public float distance;
        public bool hit;
    };
    
    public sealed class ProjectileMoveMeta : IComponent
    {
        public Vector3 origin;
        public Vector3 direction;
        public float rayPosition;
    };
}
