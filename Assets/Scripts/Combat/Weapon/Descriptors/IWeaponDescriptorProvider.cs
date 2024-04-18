namespace Scorewarrior.Test.Descriptors
{
    public interface IWeaponDescriptorProvider
    {
        float Damage { get; }
        float Accuracy { get; }
        float FireRate { get; }
        uint ClipSize { get; }
        float ReloadTime { get; }
    }
}
