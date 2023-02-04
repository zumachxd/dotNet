
using dotNet.Data;
using dotNet.Models;

namespace dotNet.Service.CharacterService
{
  public class CharacterService : ICharacterService
  {
    private static List<Character> characters = new List<Character> 
    {
        new Character(),
        new Character { Id = 1, Name = "Geraldo" }
    };

    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public CharacterService(IMapper mapper, DataContext context)
    {
      _context = context;
      _mapper = mapper;
    }
    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      var character = _mapper.Map<Character>(newCharacter);
      character.Id = characters.Max(c => c.Id) +1;
        characters.Add(character);
        serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        return serviceResponse;

    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
      var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
      try {
      var character = characters.FirstOrDefault(c => c.Id == id);
if ( character == null )
throw new Exception($"Charater with id {id} is not found");

      characters.Remove(character);

      ServiceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
} catch (Exception ex)
{
  ServiceResponse.Success = false;
  ServiceResponse.Message = ex.Message;
}
      return ServiceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      var DbContext = await _context.Characters.ToListAsync();
      serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
      return serviceResponse;
    }

    public  async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        var dbCharacter = await _context.Characters.FirstOrDefaultAsync( c => c.Id == id);
     serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
     return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
      var ServiceResponse = new ServiceResponse<GetCharacterDto>();
      try {
      var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
if ( character == null )
throw new Exception($"Charater with id {updatedCharacter.Id} is not found");

      character.Name = updatedCharacter.Name;
      character.Class = updatedCharacter.Class;
      character.HitPoints = updatedCharacter.HitPoints;
      character.Strength = updatedCharacter.Strength;
      character.Defense = updatedCharacter.Defense;
      character.Inteligence = updatedCharacter.Inteligence;

      ServiceResponse.Data = _mapper.Map<GetCharacterDto>(character);
} catch (Exception ex)
{
  ServiceResponse.Success = false;
  ServiceResponse.Message = ex.Message;
}
      return ServiceResponse;
    }
  }
}