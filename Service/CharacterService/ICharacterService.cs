namespace dotNet.Service.CharacterService
{
  public interface ICharacterService
    {
      Task<List<Character>> GetAllCharacter();

      Task<Character> GetCharacterById(int id);

      Task<List<Character>> AddCharacter(Character newCharacter);
    }
}