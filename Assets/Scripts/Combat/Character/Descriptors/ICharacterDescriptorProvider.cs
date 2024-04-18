namespace Scorewarrior.Test.Descriptors
{
    public interface ICharacterDescriptorProvider
    {
        float Accuracy { get; }
        float Dexterity { get; }
        float MaxHealth { get; }
        float MaxArmor { get; }
        float AimTime { get; }
    }
}
