using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishGrid.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {

        }
        public DbSet<Tenants> Tenant { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Suggestion> Suggestion { get; set; }
        public DbSet<Votes> Votes { get; set; }
        public DbSet<Reply> Replies { get; set; }
        
        
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Tenants>()
                 .HasOne(c => c.User)
                 .WithOne(e => e.Tenant)
                 .HasForeignKey<User>(b => b.TenantId);

            modelBuilder.Entity<Status>()
                 .HasOne(c => c.Suggestion)
                 .WithOne(e => e.Status)
                 .HasForeignKey<Suggestion>(b => b.StatusId);

            modelBuilder.Entity<Role>()
                 .HasOne<User>(c => c.User)
                 .WithOne(s => s.Role)
                 .HasForeignKey<User>(b => b.RoleId);

            modelBuilder.Entity<User>()
                 .HasMany(c => c.Suggestions)
                 .WithOne(e => e.Author).IsRequired();

            modelBuilder.Entity<Votes>()
                 .HasKey(vus => new { vus.UserId, vus.SuggestionId });

            modelBuilder.Entity<Votes>()
                .HasOne(vus => vus.User)
                .WithMany(u => u.VotesUserSuggestions)
                .HasForeignKey(vus => vus.UserId);

            modelBuilder.Entity<Votes>()
                .HasOne(vus => vus.Suggestion)
                .WithMany(s => s.VotesUserSuggestions)
                .HasForeignKey(vus => vus.SuggestionId);

            modelBuilder.Entity<Suggestion>()
                .HasMany(r => r.Replies)
                .WithOne(s => s.Suggestion).IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(r => r.Replies)
                .WithOne(s => s.Author).IsRequired();



            modelBuilder.Entity<User>().HasIndex(c=>c.RoleId).IsUnique(false);
            modelBuilder.Entity<User>().HasIndex(c => c.TenantId).IsUnique(false);
            modelBuilder.Entity<Suggestion>().HasIndex(c => c.StatusId).IsUnique(false);

            base.OnModelCreating(modelBuilder);
        }        
    }
}
