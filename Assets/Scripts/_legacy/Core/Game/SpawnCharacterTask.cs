using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior
{
    public struct SpawnCharacterTask
    {
        public GameObject Prefab { get; set; }
        public Vector3 Position { get; set; }
        public IBattlefield Battlefield { get; set; }
        public ETeam Team { get; set; }
        public uint Sector { get; set; }
    };
}
