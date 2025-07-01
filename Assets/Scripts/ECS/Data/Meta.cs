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
}
