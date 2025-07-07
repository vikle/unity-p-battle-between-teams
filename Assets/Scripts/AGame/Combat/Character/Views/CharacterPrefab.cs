using UnityEngine;

namespace Scorewarrior
{
    [DisallowMultipleComponent]
    public sealed class CharacterPrefab : MonoBehaviour
    {
        public Transform _hitBox;
        public GameObject _weaponPrefab;
        public Transform _weaponSlot;
    };
}
