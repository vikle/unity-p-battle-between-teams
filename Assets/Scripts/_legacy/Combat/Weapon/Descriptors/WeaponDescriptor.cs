using UnityEngine;

namespace Scorewarrior.Test.Descriptors
{
	[DisallowMultipleComponent]
    public sealed class WeaponDescriptor : MonoBehaviour
	{
		public float Damage;
		public float Accuracy;
		public float FireRate;
		public uint ClipSize;
		public float ReloadTime;
	}
}