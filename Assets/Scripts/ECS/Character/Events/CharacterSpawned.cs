using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class CharacterSpawned : IEvent
    {
        public Entity characterEntity;
    };
}
