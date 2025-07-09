using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class ReturnToPoolCommand : IEvent
    {
        public Entity entity;
    };
}
