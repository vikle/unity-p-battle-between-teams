using System;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
    [Serializable]
    public struct WeaponAttachment
    {
        public GameObject prefab;
        public Transform slot;
    }
}
