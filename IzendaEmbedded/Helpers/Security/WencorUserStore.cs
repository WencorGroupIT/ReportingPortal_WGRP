using IzendaEmbedded.Models.Security;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Wencor.DataAccess;

namespace IzendaEmbedded.Helpers.Security
{
	public class WencorUserStore<TUser> :
		IUserStore<TUser>,
		IUserLockoutStore<TUser, string>,
		IUserPasswordStore<TUser>,
		IUserClaimStore<TUser, string>,
		IUserTwoFactorStore<TUser, string>
		
		where TUser : WencorAccountUser
	{
		
		public Task<int> GetAccessFailedCountAsync(TUser user)
		{
			if (user == null) throw new ArgumentNullException("user");
			return Task.FromResult(user.AccessFailedCount);
		}

		public Task<bool> GetLockoutEnabledAsync(TUser user)
		{
			if (user == null) throw new ArgumentNullException("user");
			return Task.FromResult(user.LockoutEnabled);
		}

		public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
		{
			throw new NotImplementedException();
		}

		public Task<int> IncrementAccessFailedCountAsync(TUser user)
		{
			throw new NotImplementedException();
		}

		public Task ResetAccessFailedCountAsync(TUser user)
		{
			throw new NotImplementedException();
		}

		public Task SetLockoutEnabledAsync(TUser user, bool enabled)
		{
			throw new NotImplementedException();
		}

		public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetPasswordHashAsync(TUser user)
		{
			throw new NotImplementedException();
		}

		public Task<bool> HasPasswordAsync(TUser user)
		{
			throw new NotImplementedException();
		}

		public Task SetPasswordHashAsync(TUser user, string passwordHash)
		{
			user.PasswordHash = passwordHash;

			return Task.FromResult<object>(null);
		}

		public Task CreateAsync(TUser user)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(TUser user)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
		}

		public Task<TUser> FindByIdAsync(string userId)
		{
			var db = new WencorDataAccess();
			WencorAccountUser accountUser = null;

			var izendaUser = db.IzendaUsers.FirstOrDefault(i => i.Id.ToString() == userId);

			if (izendaUser != null)
				accountUser = new WencorAccountUser
				{
					Id = izendaUser.Id.ToString(),
					UserName = izendaUser.UserName,
					SystemAdmin = izendaUser.SystemAdmin
				};
			return Task.FromResult((TUser) accountUser);
		}

		public Task<TUser> FindByNameAsync(string userName)
		{
			var db = new WencorDataAccess();
			WencorAccountUser accountUser = null;

			var izendaUser = db.IzendaUsers.FirstOrDefault(i => i.UserName.ToUpper() == userName.ToUpper());

			if (izendaUser != null)
				accountUser = new WencorAccountUser
				{
					Id = izendaUser.Id.ToString(),
					UserName = izendaUser.UserName,
					SystemAdmin = izendaUser.SystemAdmin
				};
			return Task.FromResult((TUser) accountUser);
		}

		public Task UpdateAsync(TUser user)
		{
			throw new NotImplementedException();
		}

		public Task<bool> GetTwoFactorEnabledAsync(TUser user)
		{
			if (user == null) throw new ArgumentNullException("user");
			//hardcoding this for now.
			return Task.FromResult(false);
		}

		public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
		{
			throw new NotImplementedException();
		}

		public static WencorUserStore<TUser> Create()
		{
			return new WencorUserStore<TUser>();
		}

		public Task<IList<Claim>> GetClaimsAsync(TUser user)
		{
			IList<Claim> claims = new List<Claim>();

			return Task.FromResult(claims);
		}

		public Task AddClaimAsync(TUser user, Claim claim)
		{
			throw new NotImplementedException();
		}

		public Task RemoveClaimAsync(TUser user, Claim claim)
		{
			throw new NotImplementedException();
		}
	}
}