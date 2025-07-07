using System;
using UnityEngine;
using UniversalEntities;

namespace Scorewarrior.DebugGame
{
    [Serializable]
    public sealed class DebugViewCharacter
    {
        public Entity character;
        public GameObject instance;
        public ECharacterState state;
        public ETeam team;
        public uint sector;
        public int armor;
        public int health;
    }
}
