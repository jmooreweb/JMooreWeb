﻿using JMooreWeb.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace JMooreWeb.Data
{
   public static class DatabaseInitializer
	{
		// https://dotnetthoughts.net/seed-database-in-aspnet-core/
		public static void Seed(IServiceProvider serviceProvider)
		{
			using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
				context.Database.EnsureCreated();

				var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
				var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

				string[] roles = new string[] { "Admin", "Manager", "Employee" };
				foreach (string role in roles)
				{
					if (!context.Roles.Any(r => r.Name == role))
					{
						var adminRole = new Role
						{
							Name = role.ToLower(),
							NormalizedName = role.ToUpper(),
							ConcurrencyStamp = Guid.NewGuid().ToString()
						};
						context.Roles.Add(adminRole);
					}
				}

				var admin = new User
				{
					FirstName = "Administrator",
					LastName = null,
					Email = "jmooreweb@gmail.com",
					NormalizedEmail = "jmooreweb@gmail.com",
					UserName = "admin",
					NormalizedUserName = "admin",
					EmailConfirmed = true,
					PasswordHash = null,
					PhoneNumber = null,
					PhoneNumberConfirmed = false,
					LockoutEnabled = false,
					LockoutEnd = null,
					TwoFactorEnabled = false,
					AccessFailedCount = 0,
					SecurityStamp = Guid.NewGuid().ToString(),
					ConcurrencyStamp = Guid.NewGuid().ToString(),
					CreateDate = DateTime.Now
				};

				if (!context.Users.Any())
				{
					var password = new PasswordHasher<User>();
					var hashed = password.HashPassword(admin, "Admin@123");
					admin.PasswordHash = hashed;

					context.Users.Add(admin);
				}

				//if (!context.UserRoles.Any())
				//{
				//	var role = roleManager.FindByNameAsync("Admin");
				//	var user = userManager.FindByNameAsync("Admin");
				//	var userRole = new IdentityUserRole<int>
				//	{
				//		RoleId = role.Id,
				//		UserId = user.Id
				//	};
				//	context.UserRoles.Add(userRole);
				//}

				//await userManager.AddToRoleAsync(admin, "admin");

				context.SaveChanges();
			}
		}
	}
}
