using Microsoft.EntityFrameworkCore;
using Portafolio.Models;

namespace Portafolio.Data;

public class UserDb : DbContext
{
    public UserDb(DbContextOptions<UserDb> options) : base(options) { }

    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<AuthProvider> AuthProviders => Set<AuthProvider>();
    public DbSet<InviteToken> InviteTokens => Set<InviteToken>();
    public DbSet<JobHistory> JobHistories => Set<JobHistory>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<JobFile>JobFiles => Set<JobFile>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Job>()
            .Property(j => j.Status)
            .HasDefaultValue(Statuses.Created);

        modelBuilder.Entity<Job>()
            .Property(j => j.SubStatus)
            .HasDefaultValue(SubStatuses.WaitingForClient);
        
        modelBuilder.Entity<Job>()
            .Property(j => j.Budget)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ContactMessage>()
            .Property(c => c.Budget)
            .HasPrecision(18, 2);
        
        modelBuilder.Entity<JobFile>()
            .HasOne(jf => jf.Uploader)
            .WithMany(u => u.JobFiles)
            .HasForeignKey(jf => jf.UploaderId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<JobHistory>()
            .HasOne(jh => jh.Changer)
            .WithMany(u => u.JobHistories)
            .HasForeignKey(jh => jh.ChangerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}