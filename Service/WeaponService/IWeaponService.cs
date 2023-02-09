using dotNet.Dtos.Weapon;

namespace dotNet.Service.WeaponService
{
  public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);

    }
}