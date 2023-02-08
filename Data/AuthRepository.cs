using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace dotNet.Data
{
  public class AuthRepository : IAuthRepository
  {
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;
   
    public AuthRepository(DataContext context, IConfiguration configuration)
    {
      _configuration = configuration;
      _context = context;
    
        
    }
    public async Task<ServiceResponse<string>> Login(string username, string password)
    {
      var response = new ServiceResponse<string>();
      var user = await _context.Users
      .FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(username.ToLower()));
      if (user == null)
      {
        response.Success = false;
        response.Message = "Verify your credentials";
      }
      else if (!PasswordVerify(password, user.PasswordHash, user.PasswordSalt))
      {
        response.Success = false;
        response.Message = "Verify your credentials";
      }
      else {
        response.Data = CreatToken(user);
      }
      return response;
  }

    public async Task<ServiceResponse<int>> Register(User user, string password)
    {
      var response = new ServiceResponse<int>();
      if(await UserExists(user.UserName))
      {
        response.Success = false;
        response.Message = $"User ${user.UserName} already exists";
      }
        CreatPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

      _context.Users.Add(user);
      await _context.SaveChangesAsync();
      response.Data = user.id;
      return response;
    }

    public async Task<bool> UserExists(string username)
    {
     if(await _context.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower()))
     {
       return true;
     }
     return false;
    }

    private void CreatPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    { 
        using(var hamc = new System.Security.Cryptography.HMACSHA512())
        {
            passwordHash = hamc.Key;
            passwordSalt = hamc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool PasswordVerify(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using(var hamc = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
          var computeHash = hamc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
          return computeHash.SequenceEqual(passwordHash);
        }
    }

    private string CreatToken(User user)
    {
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName)
      };

      var appsSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
      if(appsSettingsToken is null)
       throw new Exception("AppSettings token id null");
      
      SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
      .GetBytes(appsSettingsToken));

      SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDecriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };

      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
      SecurityToken token = tokenHandler.CreateToken(tokenDecriptor);

      return tokenHandler.WriteToken(token);
    }
  }

}