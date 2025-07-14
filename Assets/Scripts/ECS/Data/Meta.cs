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
    
    public sealed class ProjectileTarget : IResettableComponent
    {
        public bool hit;
        public Entity entity;
        public Vector3 position;
        public float distance;
        
        void IResettableComponent.OnReset()
        {
            hit = false;
            entity = null;
            position = Vector3.zero;
            distance = 0f;
        }
    };
    
    public sealed class ProjectileMoveMeta : IResettableComponent
    {
        public Vector3 origin;
        public Vector3 direction;
        public float rayPosition;
        
        void IResettableComponent.OnReset()
        {
            origin = Vector3.zero;
            direction = Vector3.zero;
            rayPosition = 0f;
        }
    };
}
