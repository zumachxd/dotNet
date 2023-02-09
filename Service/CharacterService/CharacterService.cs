
using System.Security.Claims;
using dotNet.Data;

namespace dotNet.Service.CharacterService
{
  public class CharacterService : ICharacterService
  {

    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
      _context = context;
      _mapper = mapper;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
        .FindFirstValue(ClaimTypes.NameIdentifier)!);
    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      var character = _mapper.Map<Character>(newCharacter);
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
        serviceResponse.Data =
         await _context.Characters
          .Where(c => c.User!.id == GetUserId())
         .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
        return serviceResponse;

    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      try {
      var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
if ( character == null )
throw new Exception($"Charater with id {id} is not found");

      _context.Characters.Remove(character);

      await _context.SaveChangesAsync();

      serviceResponse.Data =
      await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
} catch (Exception ex)
{
  serviceResponse.Success = false;
  serviceResponse.Message = ex.Message;
}
      return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      var DbContext = await _context.Characters.Where(c => c.User!.id == GetUserId()).ToListAsync();
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
      var character =  await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
if ( character == null )
throw new Exception($"Charater with id {updatedCharacter.Id} is not found");

      character.Name = updatedCharacter.Name;
      character.Class = updatedCharacter.Class;
      character.HitPoints = updatedCharacter.HitPoints;
      character.Strength = updatedCharacter.Strength;
      character.Defense = updatedCharacter.Defense;
      character.Inteligence = updatedCharacter.Inteligence;

      await _context.SaveChangesAsync();
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