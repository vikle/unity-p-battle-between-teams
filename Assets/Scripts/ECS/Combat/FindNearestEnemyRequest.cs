using UniversalEntities;

namespace Scorewarrior.ECS
{
    public sealed class FindNearestEnemyRequest : IPromise
    {
        public EPromiseState State { get; set; }
        public Entity instigator;
    };
}
