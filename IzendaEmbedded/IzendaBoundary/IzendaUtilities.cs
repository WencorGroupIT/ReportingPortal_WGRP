using IzendaEmbedded.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IzendaEmbedded.IzendaBoundary
{
	public class IzendaUtilities
	{
		public static async Task<bool> SaveIzendaUser(string token, UserDetail user)
		{
			try
			{
				var success =
					await WebApiService.Instance.PostReturnValueAsync<bool, UserDetail>("/user/integration/saveUser", user, token);
				return success;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public static async Task<UserDetail> ActivateIzendaUser(string token, UserDetail user)
		{
			try
			{
				var success =
					await WebApiService.Instance.PostReturnValueAsync<UserDetail, UserDetail>("/user/active", user, token);
				return success;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public static async Task<UserDetail> DeactivateIzendaUser(string token, UserDetail user)
		{
			try
			{
				var success =
					await WebApiService.Instance.PostReturnValueAsync<UserDetail, UserDetail>("/user/deactive", user, token);
				return success;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}


		public static async Task<List<RoleDetail>> GetRoles(string authToken)
		{
			var action = string.Empty;

			action = "role/all/";

			var roles = await WebApiService.Instance.GetAsync<List<RoleDetail>>(action, authToken);
			return roles;
		}

		public static async Task<List<UserDetail>> IzendaGetAllUsers(
			string authToken)
		{
			var action = "/user/all";
			var users = await WebApiService.Instance.GetAsync<List<UserDetail>>(action, authToken);

			return users;
		}
	}
}