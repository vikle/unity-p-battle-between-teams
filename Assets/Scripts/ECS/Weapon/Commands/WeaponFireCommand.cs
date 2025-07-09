using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class WeaponFireCommand : IEvent
    {
        public Entity weapon, owner, target;
    };
}
