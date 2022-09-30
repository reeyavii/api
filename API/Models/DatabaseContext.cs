using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using API.Models.Chat;

namespace API.Models
{
    public class DatabaseContext : IdentityDbContext<AppUser>
    {
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

      
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Conversation> Conversations { get; set; } = null!;
        public DbSet<GroupMember> GroupMembers { get; set; } = null!;
        public DbSet<UserInformation> UserInformations { get; set; } = null!;
        public DbSet<Stall> Stalls { get; set; } = null!;
        public DbSet<Partner> Partners { get; set; } = null!;
        public DbSet<Verification> Verifications { get; set; } = null!;



    }
}
