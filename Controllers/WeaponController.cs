using dotNet.Dtos.Weapon;
using dotNet.Service.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotNet.Controllers
{     
        [Authorize]
        [ApiController]
        [Route("[controller]")]
  public class WeaponController : ControllerBase
    {
    private readonly IWeaponService _weaponService;
        public WeaponController(IWeaponService weaponService)
        {
           _weaponService = weaponService;
            
        }
        
        [HttpPost]

        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await _weaponService.AddWeapon(newWeapon));

        }


        
    };
}