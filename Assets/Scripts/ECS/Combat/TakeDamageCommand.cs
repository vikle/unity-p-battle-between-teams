using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class TakeDamageCommand : IEvent
    {
        public float damage;
        public Entity target;
    };
}
