using dotNet.Data;
using dotNet.Dtos.Fight;

namespace dotNet.Service.FightService
{
  public class FightService : IFightService
    {
    private readonly DataContext _context;
        public FightService(DataContext context)
        {
          _context = context;
            
        }

    public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
    {
      var response = new ServiceResponse<AttackResultDto>();
      try
      {
      var attacker = await _context.Characters
          .Include(c => c.Weapon)
          .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
      var opponent = await _context.Characters
          .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

      if (attacker is null || opponent is null || attacker.Skills is null)
          throw new Exception("Something is wrong!");

      var castedSkill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);

      if (castedSkill is null)
        {
          response.Success = false;
          response.Message = $"{attacker.Name} doesn't know that skill!";
          return response;
        }
      int damage = castedSkill.Damage + (new Random().Next(attacker.Inteligence));
      damage -= new Random().Next(opponent.Defense);

      if (damage < 0)
        opponent.HitPoints -= damage;

      if (opponent.HitPoints <= 0)
        response.Message = $"{opponent.Name} has been defeated by {attacker.Name}";

        await _context.SaveChangesAsync();

        response.Data = new AttackResultDto
        {
          Attacker = attacker.Name,
          Opponent = opponent.Name,
          AttackerHP = attacker.HitPoints,
          OpponentHP = opponent.HitPoints,
          Damage = damage
        };                


      }
      catch(Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;

      }
      return response;
    }

    public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
    {
      var response = new ServiceResponse<AttackResultDto>();
      try
      {
      var attacker = await _context.Characters
          .Include(c => c.Weapon)
          .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
      var opponent = await _context.Characters
          .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

      if (attacker is null || opponent is null || attacker.Weapon is null)
          throw new Exception("Something is wrong!");

      int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
      damage -= new Random().Next(opponent.Defense);

      if (damage < 0)
        opponent.HitPoints -= damage;

      if (opponent.HitPoints <= 0)
        response.Message = $"{opponent.Name} has been defeated by {attacker.Name}";

        await _context.SaveChangesAsync();

        response.Data = new AttackResultDto
        {
          Attacker = attacker.Name,
          Opponent = opponent.Name,
          AttackerHP = attacker.HitPoints,
          OpponentHP = opponent.HitPoints,
          Damage = damage
        };                


      }
      catch(Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;

      }
      return response;
    }

    Task<ServiceResponse<AttackResultDto>> IFightService.SkillAttack(SkillAttackDto request)
    {
      throw new NotImplementedException();
    }
  }
}