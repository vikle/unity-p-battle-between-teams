using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class CharacterStateChanged : IEvent
    {
        public Entity character;
        public ECharacterState state;
    };
}
