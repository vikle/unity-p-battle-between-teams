using System;
using Scorewarrior.Test.Views;

namespace Scorewarrior.Test.Models
{
    public interface ICharacter : IDamageable
    {
        ICharacterPrefab Prefab { get; }
        ECharacterState State { get; }
        IBattlefield Battlefield { get; }
        ETeam Team { get; }
        uint Sector { get; }
        event Action<ICharacter> OnDamageTaken;
        event Action<ICharacter> OnStateChanged;

        void Init(IBattlefield battlefield, ETeam team, uint sector);
    }
}
