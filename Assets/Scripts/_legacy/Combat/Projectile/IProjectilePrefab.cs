using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
    public interface IProjectilePrefab
    {
        void Init(float damage, IDamageable target, Vector3 targetPosition, bool hit);
    }
}
