namespace dotNet.Models
{
  public class User
    {
        public int id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = new byte[0];

        public byte[] PasswordSalt { get; set; } = new byte[0];

        public List<Character>? Characters{ get; set; }
        
    }
}