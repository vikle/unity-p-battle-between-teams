using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class CharacterMarker : IComponent
    {
        public Entity stats, modifiers, meta;
    };
}
