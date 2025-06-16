using UnityEngine;

namespace Scorewarrior.Test.Models
{
    // Нужен любого префаба в мире, который может принимать урон
    public interface IDamageablePrefab
    {
        Vector3 HitBoxPosition { get; }
    }
}
