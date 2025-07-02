using UniversalEntities;

namespace Scorewarrior.ECS
{
    public abstract class FloatStat : IComponent
    {
        public float value;
    };    
    
    public abstract class UIntStat : IComponent
    {
        public uint value;
    };
    
    public sealed class Health : FloatStat { };
    public sealed class Armor : FloatStat { };
    public sealed class Dexterity : FloatStat { };
    public sealed class AimTime : FloatStat { };
    
    public sealed class Accuracy : FloatStat { };
    
    public sealed class Damage : FloatStat { };
    public sealed class FireRate : FloatStat { };
    public sealed class ClipSize : UIntStat { };
    public sealed class ReloadTime : FloatStat { };
    
    public sealed class Speed : FloatStat { };
};
