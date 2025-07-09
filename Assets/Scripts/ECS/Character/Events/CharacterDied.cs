using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class CharacterDied : IEvent
    {
        public Entity character;
    };
}
