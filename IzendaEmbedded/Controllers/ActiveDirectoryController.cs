using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using IzendaEmbedded.Cache;
using IzendaEmbedded.Helpers.FileHelpers;
using IzendaEmbedded.IzendaBoundary;
using IzendaEmbedded.Models;
using Microsoft.Ajax.Utilities;

namespace IzendaEmbedded.Controllers
{
	[Authorize]
	public class ActiveDirectoryController : Controller
	{
		
		public async Task<ActionResult> EditActiveDirectoryUser(string emailAddress)

		{
			
			ViewBag.SystemAdmin = false;
			var identity = (System.Security.Claims.ClaimsIdentity) User.Identity;
			var claims = identity.Claims;
			string SystemAdmin = claims.FirstOrDefault(x => x.Type == "SystemAdmin")?.Value;
			if (SystemAdmin == "True")
			{
				ViewBag.SystemAdmin = true;
			}
			var token = GetToken();
			var roles = await IzendaUtilities.GetRoles(token);

			var employeeList = CacheSystem.GetCacheItem("combinedList") as List<UserDetail>;
			var employee = (from e in employeeList
				where e.EmailAddress == emailAddress
				select e).FirstOrDefault();

			var userRoles = roles.Where(t => t.Users.Contains(employee)).ToList();
			var userRolesIds = new string[userRoles.Count];

			var length = userRoles.Count;

			for (var i = 0; i < length; i++) userRolesIds[i] = userRoles[i].Id.ToString();

			var rolesList = new MultiSelectList(roles.ToList().OrderBy(i => i.Name), "Id", "Name", userRolesIds);
			employee.RoleOptions = rolesList;

			return View(employee);
		}

		[HttpPost]
		public async Task<ActionResult> EditActiveDirectoryUser(UserDetail userDetail)
		{
			if (ModelState.IsValid)
			{
				ViewBag.SystemAdmin = false;
				var identity = (System.Security.Claims.ClaimsIdentity) User.Identity;
				var claims = identity.Claims;
				string SystemAdmin = claims.FirstOrDefault(x => x.Type == "SystemAdmin")?.Value;
				if (SystemAdmin == "True")
				{
					ViewBag.SystemAdmin = true;
				}
				
				var token = GetToken();

				userDetail.Roles = new List<RoleDetail>();

				if (userDetail.RoleIds == null)
				{
					var message = "Must select at least one role for user.";
					throw new InvalidOperationException(message);
				}

				if (userDetail.RoleIds.Count > 0)
				{
					var userRoles = new List<RoleDetail>();

					foreach (var id in userDetail.RoleIds)
					{
						var roles = await IzendaUtilities.GetRoles(token);
						var roleId = roles.FirstOrDefault(i => i.Id == Guid.Parse(id)).Id.ToString();

						var role = roles.Where(i => i.Id == Guid.Parse(roleId)).Select(v => new RoleDetail
						{
							Id = v.Id,
							Name = v.Name
						}).FirstOrDefault();

						if (roleId != null)
							try
							{
								userDetail.Roles.Add(role);
								userRoles.Add(role);
							}
							catch (Exception e)
							{
								Console.WriteLine(e);
								throw;
							}
					}

					var allRoles = await IzendaUtilities.GetRoles(token);
					var rolesToRemove = allRoles.Except(userRoles);

					foreach (var role in rolesToRemove)
						try
						{
							userDetail.Roles.Remove(role);
						}
						catch (Exception e)
						{
							Console.WriteLine(e);
							throw;
						}
				}

				try
				{
					var success = false;

					var izendaUsers = new List<UserDetail>();

					izendaUsers = await IzendaUtilities.IzendaGetAllUsers(token);

					var currentUser = izendaUsers.FirstOrDefault(x => x.EmailAddress == userDetail.EmailAddress);
					if (currentUser == null)
					{
						success = await IzendaUtilities.SaveIzendaUser(token, userDetail);
						if (success)
						{
							var combinedList = CacheSystem.GetCacheItem("combinedList") as List<UserDetail>;
							var duplicateUserDetail =
								combinedList.FirstOrDefault(e => e.EmailAddress == userDetail.EmailAddress);
							combinedList.Remove(duplicateUserDetail);

							combinedList.Add(userDetail);

							return PartialView("ActiveDirectoryList", combinedList);
						}
					}

					if (currentUser != null && currentUser.Active != userDetail.Active)
					{
						if (currentUser.Active && !userDetail.Active)
							currentUser = await IzendaUtilities.DeactivateIzendaUser(token, userDetail);

						if (!currentUser.Active && userDetail.Active)
							currentUser = await IzendaUtilities.ActivateIzendaUser(token, userDetail);

						if (currentUser.Active == userDetail.Active) success = await IzendaUtilities.SaveIzendaUser(token, currentUser);
					}

					if (currentUser.Active == userDetail.Active) success = await IzendaUtilities.SaveIzendaUser(token, currentUser);

					if (success)
					{
						var combinedList = CacheSystem.GetCacheItem("combinedList") as List<UserDetail>;
						var duplicateUserDetail =
							combinedList.FirstOrDefault(e => e.EmailAddress == userDetail.EmailAddress);
						combinedList.Remove(duplicateUserDetail);

						combinedList.Add(userDetail);

						return PartialView("ActiveDirectoryList", combinedList);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;

				}
				return View(userDetail);
				
			}
			else
			{
				ViewBag.SystemAdmin = false;
				var identity = (System.Security.Claims.ClaimsIdentity) User.Identity;
				var claims = identity.Claims;
				string SystemAdmin = claims.FirstOrDefault(x => x.Type == "SystemAdmin")?.Value;
				if (SystemAdmin == "True")
				{
					ViewBag.SystemAdmin = true;
				}
				
				var token = GetToken();
				var roles = await IzendaUtilities.GetRoles(token);

				var employeeList = CacheSystem.GetCacheItem("combinedList") as List<UserDetail>;
				var employee = (from e in employeeList
					where e.EmailAddress == userDetail.EmailAddress
					select e).FirstOrDefault();

				var userRoles = roles.Where(t => t.Users.Contains(employee)).ToList();
				var userRolesIds = new string[userRoles.Count];

				var length = userRoles.Count;

				for (var i = 0; i < length; i++) userRolesIds[i] = userRoles[i].Id.ToString();

				var rolesList = new MultiSelectList(roles.ToList().OrderBy(i => i.Name), "Id", "Name", userRolesIds);
				employee.RoleOptions = rolesList;

				return View(employee);
			}
		}

		public async Task<ActionResult> Parse(string fileName)
		{
			try

			{
				var token = GetToken();
				List<UserDetail> izendaUsers;
				List<UserDetail> combinedList;
				try
				{
					izendaUsers = await IzendaUtilities.IzendaGetAllUsers(token);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}

				var cachedList = CacheSystem.GetCacheItem("combinedList") as List<UserDetail>;
				if (cachedList == null)
				{
					var cachedFileString = CacheUpdate.GetCSV();
					var delimitedByLine =
						cachedFileString.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries).ToArray();

					var employeeList = delimitedByLine.Select((x, index) =>
					{
						var delimitedByTab = x.Split(new[] {"\t"}, StringSplitOptions.None);
						return new UserDetail
						{
							DomainName = delimitedByTab[0].Replace("\"", ""),
							Name = delimitedByTab[1].ToUpper(),
							UserName = delimitedByTab[2],
							FirstName = delimitedByTab[3],
							LastName = delimitedByTab[4],
							EmailAddress = delimitedByTab[5],
							PhysicalDeliveryOfficeName = delimitedByTab[6]
						};
					}).ToList();

					foreach (var i in izendaUsers)
					{
						i.Name = (i.FirstName + " " + i.LastName).ToUpper();
						var containsUser = employeeList.Any(e => e.Name == i.Name);
						if (containsUser)
						{
							var userToRemove = employeeList.FirstOrDefault(e => e.Name == i.Name);

							if (userToRemove != null) employeeList.Remove(userToRemove);
						}
					}

					combinedList = employeeList.Concat(izendaUsers).DistinctBy(i => i.Name).ToList();

					CacheSystem.AddCacheItem("combinedList", combinedList, 60);
				}
				else
				{
					combinedList = cachedList.Concat(izendaUsers).DistinctBy(i => i.EmailAddress).ToList();
					CacheSystem.AddCacheItem("combinedList", combinedList, 60);
				}

				return PartialView("ActiveDirectoryList", combinedList);
			}
			catch (Exception ex)

			{
				throw ex;
			}
		}

		private static string GetToken()
		{
			var user = new UserInfo
			{
				TenantUniqueName = ConfigurationManager.AppSettings["SystemTenant"],
				UserName = ConfigurationManager.AppSettings["IzendaAdminUser"]
			};

			var token = IzendaTokenAuthorization.GetToken(user);

			return token;
		}
	}
}