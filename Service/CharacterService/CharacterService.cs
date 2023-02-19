
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
      character.User = await _context.Users.FirstOrDefaultAsync(u => u.id == GetUserId());
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
      var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.id == GetUserId());
if ( character == null )
throw new Exception($"Charater with id {id} is not found");

      _context.Characters.Remove(character);

      await _context.SaveChangesAsync();

      serviceResponse.Data =
      await _context.Characters
      .Where(c => c.User!.id == GetUserId())
      .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
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
      var DbContext = await _context.Characters
        .Include(c => c.Weapon)
        .Include(c => c.Skills)
        .Where(c => c.User!.id == GetUserId()).ToListAsync();
      serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
      return serviceResponse;
    }

    public  async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        var dbCharacter = await _context.Characters
         .Include(c => c.Weapon)
         .Include(c => c.Skills)
         .FirstOrDefaultAsync( c => c.Id == id && c.User!.id == GetUserId());
     serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
     return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
      var ServiceResponse = new ServiceResponse<GetCharacterDto>();
      try {
      var character = 
      await _context.Characters
      .Include(c => c.User)
      .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
if ( character == null)
throw new Exception($"Charater with id {updatedCharacter.Id} is not found");
else if ( character.User!.id != GetUserId() )
throw new Exception($"User without permission for Charater with id {updatedCharacter.Id}");

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

    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
      var response = new ServiceResponse<GetCharacterDto>();
      try
      {
        var character = await _context.Characters
                   .Include(c => c.Skills)
                   .Include(c => c.Weapon)
                   .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId &&
                      c.User!.id == GetUserId());
                
                if(character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found";
                    return response;
                }
        var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId );
                if(skill is null)
                {
                    response.Success = false;
                    response.Message = "Skill not found";
                    return response;
                } 
                character.Skills!.Add(skill);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);         
        
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
      
      }

      return response;
    }
  }
}