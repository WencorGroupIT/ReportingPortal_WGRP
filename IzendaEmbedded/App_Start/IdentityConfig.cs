using System;
using System.Security.Claims;
using IzendaEmbedded.Helpers.Security;
using IzendaEmbedded.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace IzendaEmbedded
{
	// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
	public class ApplicationUserManager : UserManager<WencorAccountUser>
	{
		//NOTE: the ApplicationUserManager is only implemented to support the ApplicationSignInManager
		public ApplicationUserManager(IUserStore<WencorAccountUser> store) : base(store)
		{
		}

		public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
			IOwinContext context)
		{
			//var manager = new ApplicationUserManager(new UserStore<WencorAccountUser>(context.Get<ApplicationDbContext>()));

			var manager = new ApplicationUserManager(WencorUserStore<WencorAccountUser>.Create());

			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
				manager.UserTokenProvider =
					new DataProtectorTokenProvider<WencorAccountUser>(dataProtectionProvider.Create("ASP.NET Identity"));

			return manager;
		}
	}

	// Configure the application sign-in manager which is used in this application.
	public class ApplicationSignInManager : SignInManager<WencorAccountUser, string>
	{
		public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
			: base(userManager, authenticationManager)
		{
		}

		public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options,
			IOwinContext context)
		{
			return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
		}

		public override async Task<ClaimsIdentity> CreateUserIdentityAsync(WencorAccountUser user)
		{
			var identity = await base.CreateUserIdentityAsync(user);
			identity.AddClaim(new System.Security.Claims.Claim("SystemAdmin", user.SystemAdmin.ToString()));
			identity.AddClaim(new System.Security.Claims.Claim("UserName", user.UserName));
			return identity;
		}
	}
}