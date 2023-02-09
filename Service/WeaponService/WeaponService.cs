using System.Security.Claims;
using dotNet.Data;
using dotNet.Dtos.Weapon;

namespace dotNet.Service.WeaponService
{
  public class WeaponService : IWeaponService
    {
    public DataContext _context { get; }
    public IHttpContextAccessor _httpContextAccessor { get; }
    private readonly IMapper _mapper;
    public WeaponService(DataContext contex, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _context = contex;
        
    }
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                   .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                      c.User!.id == int.Parse(_httpContextAccessor.HttpContext!.User
                        .FindFirstValue(ClaimTypes.NameIdentifier)!));
                
                if(character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found";
                    return response;
                }   
                var Weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };   
                _context.Weapons.Add(Weapon);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCharacterDto>(character);      


            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}