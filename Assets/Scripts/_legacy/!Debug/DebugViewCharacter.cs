using System;
using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test._Debug
{
    [Serializable]
    public sealed class DebugViewCharacter
    {
        public ICharacter character;
        public GameObject Instance;
        public ECharacterState State;
        public ETeam Team;
        public uint Sector;
    }
}
