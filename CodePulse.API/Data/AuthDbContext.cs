using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
	public class AuthDbContext : IdentityDbContext
	{
		public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
		{
		}

		protected AuthDbContext()
		{
		}


		
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			var readerRoleId = "ab18d4cb-0862-4752-885d-36477bdfb60c";
			var writerRoleId = "14df1079-3445-4954-a4f8-c543db582c6a";


			//reader role for public (only GET)
			//admin role for admin 

			var roles = new List<IdentityRole> {
				new IdentityRole()
				{
					Id = readerRoleId,
					Name = "Reader",
					NormalizedName = "Reader".ToUpper(),
					ConcurrencyStamp = readerRoleId
				},
				new IdentityRole ()
				{
					Id = writerRoleId,
					Name = "Writer",
					NormalizedName = "Writer".ToUpper(),
					ConcurrencyStamp = writerRoleId
				}
			};


			//seed the roles
			builder.Entity<IdentityRole>().HasData(roles);

			//create an admin user.
			var adminUserId = "5dd28e82-8f21-44b4-a5ea-02b0f918c9e8";
			var admin = new IdentityUser()
			{
				Id = adminUserId,
				UserName = "leo@muller.co.il",
				Email = "leo@muller.co.il",
				NormalizedUserName = "leo@muller.co.il".ToUpper(),
				NormalizedEmail = "leo@muller.co.il".ToUpper()

			};

			admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

			builder.Entity<IdentityUser>().HasData(admin);

			//give roles to admin user.
			var adminRoles = new List<IdentityUserRole<string>>()
			{
				new()
				{
					UserId = adminUserId,
					RoleId = readerRoleId
				},
				new()
				{
					UserId = adminUserId,
					RoleId = writerRoleId
				}
			};

			builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
			

		}

	}
}
