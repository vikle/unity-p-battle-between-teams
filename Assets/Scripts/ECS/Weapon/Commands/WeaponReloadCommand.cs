using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class WeaponReloadCommand : IEvent
    {
        public Entity weapon;
        public Entity owner;
    };
}
