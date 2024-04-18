using System;
using Scorewarrior.Test.Views;

namespace Scorewarrior.Test.Models
{
    public interface ICharacter : IDamageable
    {
        ICharacterPrefab Prefab { get; }
        CharacterState State { get; }
        IBattlefield Battlefield { get; }
        Team Team { get; }
        uint Sector { get; }
        event Action<ICharacter> OnDamageTaken;
        event Action<ICharacter> OnStateChanged;

        void Init(IBattlefield battlefield, Team team, uint sector);
    }
}
