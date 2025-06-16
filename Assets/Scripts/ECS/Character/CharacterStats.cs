using UniversalEntities;

namespace Scorewarrior.ECS
{
    public abstract class CharacterStat : IComponent
    {
        public float Value;
    };
    
    public sealed class Accuracy : CharacterStat { };
    public sealed class Dexterity : CharacterStat { };
    public sealed class Health : CharacterStat { };
    public sealed class Armor : CharacterStat { };
    public sealed class AimTime : CharacterStat { };
}
