namespace  Scorewarrior.Test.Models
{
    public interface IBattlefield
    {
        void Clear();
        void RegisterCharacter(ICharacter character);
        bool TryGetNearestAliveEnemy(ICharacter character, out ICharacter target);
        ICharacter[] GetAllCharacters();
        ICharacter[] GetCharactersOfTeam(Team team);
        bool TeamIsAlive(Team team);
        bool TeamIsDead(Team team);
    }
}
