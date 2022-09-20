using INSEE.KIOSK.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Company> Companies { get; set; }
        //public DbSet<Location> Locations { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Guest_Master> Guest_Master { get; set; }
        public DbSet<Contractor_Master> Contractors_Master { get; set; }
        public DbSet<Guest_Detail> Guest_Details { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Course_Question> Course_Questions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Guest_Detail_Attempt> Guest_Detail_Attempts { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<VW_DailyGuestSummary> VW_DailyGuestSummary { get; set; }
        public DbSet<VW_GuestSummary> VW_GuestSummary { get; set; }

        protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("M_User").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("M_User_Role");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("T_User_Login");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("T_User_Claims");
            modelBuilder.Entity<IdentityRole>().ToTable("M_Role");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("T_User_Token");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("M_Role_Claims");
            // modelBuilder.Entity<RefreshToken>().ToTable("T_RefreshToken");

            modelBuilder.Entity<Course_Question>().HasKey(u => new
            {
                u.FK_CourseCode,
                u.FK_QuestionCode
            });

            modelBuilder.Entity<VW_DailyGuestSummary>().ToView(nameof(VW_DailyGuestSummary)).HasKey(t => new { t.PersonNIC,t.inserted});
            modelBuilder.Entity<VW_GuestSummary>().ToView(nameof(VW_GuestSummary)).HasKey(t => new { t.PersonNIC, t.inserted });

        }
    }

    public class ApplicationDbContextSeed
    {
        public static async Task SeedEssentialsAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(CommonResources.Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(CommonResources.Roles.Content_Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(CommonResources.Roles.Report_Viewer.ToString()));

            //Seed Default User
            var defaultUser = new ApplicationUser { UserName = CommonResources.default_username, Email = CommonResources.default_email, EmailConfirmed = true, PhoneNumberConfirmed = true };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, CommonResources.default_password);
                await userManager.AddToRoleAsync(defaultUser, CommonResources.default_role.ToString());

                
            }
        }
    }
}
