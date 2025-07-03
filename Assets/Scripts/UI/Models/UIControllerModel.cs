using System;

namespace Scorewarrior.UI
{
    public sealed class UIControllerModel
    {
        public Action<EGameState> onGameStateChanged;
        
        public void Call_OnGameStateChanged(EGameState newState)
        {
            onGameStateChanged?.Invoke(newState);
        }

        public void Dispose()
        {
            onGameStateChanged = null;
        }
    };
}
