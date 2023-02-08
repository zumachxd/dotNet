using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace dotNet.Controllers
{   
    [Authorize]
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
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get() 
        {
         return Ok( await _characterService.GetAllCharacter());
        }

        [HttpGet("{id}")]
         public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id) 
        {
         return Ok(await _characterService.GetCharacterById(id));
        }

    [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto NewCharacter)
        {
          
          return Ok(await _characterService.AddCharacter(NewCharacter));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
          var response = await _characterService.UpdateCharacter(updatedCharacter);
          if (response.Data is null)
          {
            return NotFound(response);
          }
          return Ok(response);
        }

         [HttpDelete("{id}")]
         public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> DeleteCharacter(int id) 
        {
         var response = await _characterService.DeleteCharacter(id);
          if (response.Data is null)
          {
            return NotFound(response);
          }
          return Ok(response);
        }
        
    }
}