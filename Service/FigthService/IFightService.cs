using dotNet.Dtos.Fight;

namespace dotNet.Service.FigthService
{
  public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);

        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);

    }
}