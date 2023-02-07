using dotNet.Data;
using dotNet.Dtos;
using dotNet.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace dotNet.Controllers
{
  [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase 
    {
       private readonly IAuthRepository _authRepo;

       public  AuthController(IAuthRepository authRepo)
       {
        _authRepo = authRepo;
       }    

[HttpPost("Register")]
       public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
       {
        var response = await _authRepo.Register(
            new User { UserName = request.Username }, request.Password
        );
        if(!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
       }   

       [HttpPost("Login")]
       public async Task<ActionResult<ServiceResponse<int>>> Login(LoginDto request)
       {
        var response = await _authRepo.Login(request.Username, request.Password);
        if(!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
       }    
 
    }
}