using UnityEngine;

namespace Scorewarrior.Test.Descriptors
{
    [DisallowMultipleComponent]
    public sealed class WeaponDescriptorProvider : MonoBehaviour
    {
        [SerializeField]WeaponDescriptor _descriptor;

        public Vector2 modificationRange = new Vector2(1f, 2f);
        public int modifiersCount = 2;
    };
}
