namespace dotNet.Data
{
  public class DataContext: DbContext
    {
         protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();

    }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Character> Characters => Set<Character>();
    }
}