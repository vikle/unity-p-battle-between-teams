using System;
using UniversalEntities;

namespace Scorewarrior.DebugGame
{
    public sealed class DebugInspectorModel
    {
        public Action<EGameState> onGameStateChanged;
        public Action<Entity> onCharacterSpawned;
        public Action<Entity> onCharacterStateChanged;
        public Action<Entity> onCharacterDamageTaken;

        public void Dispose()
        {
            onGameStateChanged = null;
            onCharacterSpawned = null;
            onCharacterStateChanged = null;
            onCharacterDamageTaken = null;
        }
    };
}
