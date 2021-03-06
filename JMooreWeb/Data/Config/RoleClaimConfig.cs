﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JMooreWeb.Data.Config
{
	public class RoleClaimConfig : IEntityTypeConfiguration<IdentityRoleClaim<int>>
	{
		public void Configure(EntityTypeBuilder<IdentityRoleClaim<int>> builder)
		{
			builder.ToTable("RoleClaims");
		}
	}
}
