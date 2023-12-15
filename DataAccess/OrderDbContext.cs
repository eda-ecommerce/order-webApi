public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Payments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        

    }
}