using dotNet.Dtos.Weapon;

namespace dotNet
{
  public class AutoMapperPorfile : Profile
    {
        public AutoMapperPorfile()
        {
            CreateMap<Character,GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();
        }
    }
}