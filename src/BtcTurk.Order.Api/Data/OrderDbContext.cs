using BtcTurk.Order.Api.NotificationHistories;
using BtcTurk.Order.Api.Orders;
using Microsoft.EntityFrameworkCore;

namespace BtcTurk.Order.Api.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
    public DbSet<Orders.Order> Orders => Set<Orders.Order>();
    public DbSet<OrderNotification> OrderNotifications => Set<OrderNotification>();
    public DbSet<NotificationHistory> NotificationHistories => Set<NotificationHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<NotificationHistory>().HasIndex(p =>new { p.OrderId, p.UserId});
        modelBuilder.Entity<Orders.Order>().HasKey(p =>new {p.UserId});
        modelBuilder.Entity<Orders.Order>().HasIndex(p =>new {p.Id});
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: BaseEntity, State: EntityState.Added or EntityState.Modified });
        
        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}