using UnityEngine;

namespace Scorewarrior.Descriptors
{
    [RequireComponent(typeof(CharacterPrefab))]
    public sealed class CharacterAnimationDescriptor : MonoBehaviour
    {
        [Space]
        public Animator _animator;

        [Header("Parameters Names")]
        public string _aimingName = "aiming";
        public string _reloadingName = "reloading";
        public string _shootName = "shoot";
        public string _reloadTimeName = "reload_time";
        public string _dieName = "die";

        [Header("Settings")]
        public float _reloadAnimationLength = 3.3f;
    };
}
