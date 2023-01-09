using Microsoft.AspNetCore.Mvc;

namespace dotNet.Controllers
{
  [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
      public CharacterController(ICharacterService characterService)
      {
     _characterService = characterService;
    }
    private readonly ICharacterService _characterService;

    [HttpGet("GetAll")]
        public async Task<ActionResult<List<Character>>> Get() 
        {
         return Ok( await _characterService.GetAllCharacter());
        }

        [HttpGet("{id}")]
         public async Task<ActionResult<Character>> GetSingle(int id) 
        {
         return Ok(await _characterService.GetCharacterById(id));
        }

    [HttpPost]
        public async Task<ActionResult<List<Character>>> AddCharacter(Character NewCharacter)
        {
          
          return Ok(await _characterService.AddCharacter(NewCharacter));
        }
        
    }
}