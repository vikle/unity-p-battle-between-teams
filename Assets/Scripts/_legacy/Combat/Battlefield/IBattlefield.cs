namespace  Scorewarrior.Test.Models
{
    public interface IBattlefield
    {
        void Clear();
        void RegisterCharacter(ICharacter character);
        bool TryGetNearestAliveEnemy(ICharacter character, out ICharacter target);
        ICharacter[] GetAllCharacters();
        ICharacter[] GetCharactersOfTeam(ETeam team);
        bool TeamIsAlive(ETeam team);
        bool TeamIsDead(ETeam team);
    }
}
