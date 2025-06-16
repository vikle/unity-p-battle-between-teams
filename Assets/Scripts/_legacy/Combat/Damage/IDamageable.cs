using System;

namespace Scorewarrior.Test.Models
{
    // Любая модель, которая может принимать урон
    public interface IDamageable
    {
        bool IsAlive { get; }
        float Armor { get; }
        float Health { get; }
        
        void TakeDamage(float damage);
    }
}
