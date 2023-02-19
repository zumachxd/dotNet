using dotNet.Dtos.Fight;
using Microsoft.AspNetCore.Mvc;

namespace dotNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
  public class FightController : ControllerBase
    {
    private readonly IFightService _fightServices;
        public FightController(IFightService fightServices)
        {
      _fightServices = fightServices;
            
        }

      [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto response)
        {
          return Ok(await _fightServices.WeaponAttack(response));
        }
      [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto response)
        {
          return Ok(await _fightServices.SkillAttack(response));
        }

    }
}