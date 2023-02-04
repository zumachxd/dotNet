namespace dotNet
{
  public class AutoMapperPorfile : Profile
    {
        public AutoMapperPorfile()
        {
            CreateMap<Character,GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
        }
    }
}