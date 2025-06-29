using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class CharacterStateChanged : IEvent
    {
        public Entity characterEntity;
        public ECharacterState newState;
    };
}
