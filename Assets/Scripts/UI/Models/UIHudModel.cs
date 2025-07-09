using System;
using UniversalEntities;

namespace Scorewarrior.UI
{
    public sealed class UIHudModel
    {
        public Action<Entity> onCharacterSpawned;
        public Action<Entity> onCharacterDamageTaken;
        public Action<Entity> onCharacterDie;

        public void Call_OnCharacterSpawned(Entity characterEntity)
        {
            onCharacterSpawned?.Invoke(characterEntity);
        }
        
        public void Call_OnCharacterDamageTaken(Entity characterEntity)
        {
            onCharacterDamageTaken?.Invoke(characterEntity);
        }
        
        public void Call_OnCharacterDie(Entity characterEntity)
        {
            onCharacterDie?.Invoke(characterEntity);
        }
        
        public void Dispose()
        {
            onCharacterSpawned = null;
            onCharacterDamageTaken = null;
            onCharacterDie = null;
        }
    };
}
