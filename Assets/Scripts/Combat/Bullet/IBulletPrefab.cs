using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
    public interface IBulletPrefab
    {
        void Init(float damage, IDamageable target, Vector3 targetPosition, bool hit);
    }
}
