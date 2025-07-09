using UniversalEntities;

namespace Scorewarrior.ECS
{
    public class CharacterDamageTaken : IEvent
    {
        public Entity character;
    };
}
