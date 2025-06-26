using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class CharacterMarker : IComponent
    {
        public Entity statsEntity;
        public Entity modifiersEntity;
        public Entity metaEntity;
    };
}
