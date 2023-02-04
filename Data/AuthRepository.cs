namespace dotNet.Data
{
  public class AuthRepository : IAuthRepository
  {
    private readonly DataContext _context;
   
    public AuthRepository(DataContext context)
    {
      _context = context;
    
        
    }
    public Task<ServiceResponse<string>> Login(string user, string password)
    {
      throw new NotImplementedException();
    }

    public async Task<ServiceResponse<int>> Register(User user, string password)
    {
        CreatPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

      _context.Users.Add(user);
      await _context.SaveChangesAsync();
      var response = new ServiceResponse<int>();
      response.Data = user.id;
      return response;
    }

    public Task<bool> UserExists(string username)
    {
      throw new NotImplementedException();
    }

    private void CreatPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    { 
        using(var hamc = new System.Security.Cryptography.HMACSHA512())
        {
            passwordHash = hamc.Key;
            passwordSalt = hamc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
  }

}