using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class GameStateChanged : IEvent
    {
        public EGameState newState;
    };
}
