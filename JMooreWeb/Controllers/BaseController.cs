﻿using JMooreWeb.Data;
using JMooreWeb.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMooreWeb.Controllers
{
   public class BaseController : Controller
	{
		protected readonly UserManager<User> _userManager;
		protected readonly RoleManager<Role> _roleManager;
		//protected readonly IPhotoRepository _repo;
		protected readonly DatabaseContext _context;
		protected readonly IFileProvider _fileProvider;

		public BaseController(/*IPhotoRepository repo*/)
		{
			//_repo = repo;
		}

		public BaseController(UserManager<User> userManager/*, IPhotoRepository repo*/)
		{
			_userManager = userManager;
			//_repo = repo;
		}

		public BaseController(UserManager<User> userManager, DatabaseContext context/*, IPhotoRepository repo*/)
		{
			_userManager = userManager;
			_context = context;
			//_repo = repo;
		}

		public BaseController(UserManager<User> userManager, RoleManager<Role> roleManager, DatabaseContext context/*, IPhotoRepository repo*/)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
			//_repo = repo;
		}

		public BaseController(UserManager<User> userManager, RoleManager<Role> roleManager, DatabaseContext context, /*IPhotoRepository repo, */IFileProvider fileProvider)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
			//_repo = repo;
			_fileProvider = fileProvider;
		}

		protected void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		protected async Task<User> GetCurrentUserAsync()
		{
			//return await _userManager.GetUserAsync(HttpContext.User);
			return await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id.ToString() == _userManager.GetUserId(HttpContext.User));
		}

		protected int GetCurrentUserId()
		{
			var task = GetCurrentUserAsync();

			var user = task.Result;

			if (user == null)
			{
				throw new Exception("Unable to get id of current user.");
			}

			return user.Id;
		}

		protected bool HasPermission(string permissionName)
		{
			var task = GetCurrentUserAsync();

			var user = task.Result;

			if (user == null)
			{
				return false;
			}

			var permission = _context.Permissions.Include(p => p.RolePermissions).FirstOrDefault(p => p.Name == permissionName);

			if (permission == null)
			{
				return false;
			}

			var roleIds = new List<int>();

			foreach (var rolePermission in permission.RolePermissions)
			{
				roleIds.Add(rolePermission.RoleId);
			}

			var userRoles = user.Roles;

			foreach (var userRole in userRoles)
			{
				if (roleIds.Contains(userRole.RoleId))
				{
					return true;
				}
			}

			return false;
		}

		public override UnauthorizedResult Unauthorized()
		{
			var result = new UnauthorizedResult();

			return result;
		}

		public IActionResult AccessDenied()
		{
			return View("AccessDenied");
		}

		protected IActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction(nameof(HomeController.Index), "Home");
			}
		}
	}
}
