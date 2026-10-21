using Microsoft.EntityFrameworkCore;
using Gym_Managemnt_System_Core.Entities;
using Gym_Managemnt_System_Core.Enums;

namespace GymManagementSystem.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Member> Members => Set<Member>();
        public DbSet<Plan> Plans => Set<Plan>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<CheckIn> CheckIns => Set<CheckIn>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Notes From me : Fast Scan Look up , what i mean --> databases refuses duplicated tokens
            modelBuilder.Entity<Member>().HasIndex(m => m.QrToken).IsUnique();


            //find a username , so we avoid two player or members have the same username 
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();



            modelBuilder.Entity<Plan>().Property(p => p.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Subscription>().Property(s => s.PriceCharged).HasPrecision(18, 2);
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);

            //Deleting , Does not mean Removing . 
            // We cannot Remove player's Subscriptions and plans . 
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Members).WithMany(m => m.Subscriptions)
                .HasForeignKey(s => s.MemberId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Plan).WithMany(s => s.Subscriptions)
                .HasForeignKey(s => s.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Subscription).WithMany(s => s.Payments)
                .HasForeignKey(p => p.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
           .HasOne(p => p.ReceivedBy).WithMany()
           .HasForeignKey(p => p.ReceivedByUserId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CheckIn>()
            .HasOne(c => c.Member).WithMany(m => m.CheckIns)
            .HasForeignKey(c => c.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.CreatedBy).WithMany()
                .HasForeignKey(s => s.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Member>().Property(m => m.FullName).HasMaxLength(100);
            modelBuilder.Entity<Member>().Property(m => m.Phone).HasMaxLength(20);
            modelBuilder.Entity<Member>().Property(m => m.QrToken).HasMaxLength(64);
            modelBuilder.Entity<User>().Property(u => u.UserName).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(200);
            modelBuilder.Entity<Plan>().Property(p => p.Name).HasMaxLength(100);


        }
    }
}

