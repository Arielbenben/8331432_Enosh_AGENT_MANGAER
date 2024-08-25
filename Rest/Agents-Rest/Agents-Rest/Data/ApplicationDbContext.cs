using Agents_Rest.Model;
using Microsoft.EntityFrameworkCore;

namespace Agents_Rest.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {
            
        }

        public DbSet<AgentModel> Agents { get; set; }
        public DbSet<TargetModel> Targets { get; set; }
        public DbSet<MissionModel> Missions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MissionModel>()
                .HasOne(mission => mission.Agent)
                .WithMany()
                .HasForeignKey(mission => mission.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MissionModel>()
                .HasOne(mission => mission.Target)
                .WithMany()
                .HasForeignKey(mission => mission.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MissionModel>()
                .Property(m => m.Status)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<AgentModel>()
                .Property(a => a.Status)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<TargetModel>()
                .Property(a => a.Status)
                .HasConversion<string>()
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

    }
}
