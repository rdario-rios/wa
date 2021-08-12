using ContactsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<SkillLevel> SkillLevels { get; set; }
        public DbSet<SkillDefinition> SkillDefinitions { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().ToTable("Contact");
            modelBuilder.Entity<SkillLevel>().ToTable("SkillLevel");
            modelBuilder.Entity<SkillDefinition>().ToTable("SkillDefinition");
            modelBuilder.Entity<Skill>().ToTable("Skill");
        }
        public DbSet<ContactsAPI.Models.Contact> Contact { get; set; }
        public DbSet<ContactsAPI.Models.SkillLevel> SkillLevel { get; set; }
        public DbSet<ContactsAPI.Models.SkillDefinition> SkillDefinition { get; set; }
        public DbSet<ContactsAPI.Models.Skill> Skill { get; set; }
    }
}
