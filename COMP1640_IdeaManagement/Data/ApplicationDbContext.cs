using COMP1640_IdeaManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace COMP1640_IdeaManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<IdeaUser> IdeaUser { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<IdeaImage> IdeaImages { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Dislike> Dislikes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName[6..]);
                }
            }

            builder.Entity<IdeaUser>()
            .HasKey(c => new { c.IdeaId, c.CommentId });

            builder.Entity<IdeaUser>()
                        .HasOne(c => c.Comment)
                        .WithMany()
                        .HasForeignKey(c => c.CommentId)
                        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<IdeaUser>()
                        .HasOne(c => c.Idea)
                        .WithMany()
                        .HasForeignKey(c => c.IdeaId)
                        .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
