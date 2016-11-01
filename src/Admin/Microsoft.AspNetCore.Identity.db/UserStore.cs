using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using cnodejs.BLL;
using cnodejs.Model;
using bll = cnodejs.BLL;

namespace Microsoft.AspNetCore.Identity.db {
	public partial class UserStore : IUserLoginStore<UsersInfo>,
		IUserRoleStore<UsersInfo>,
		IUserClaimStore<UsersInfo>,
		IUserPasswordStore<UsersInfo>,
		IUserSecurityStampStore<UsersInfo>,
		IUserEmailStore<UsersInfo>,
		IUserLockoutStore<UsersInfo>,
		IUserPhoneNumberStore<UsersInfo>,
		//IQueryableUserStore<UsersInfo>,
		IUserTwoFactorStore<UsersInfo>,
		IUserAuthenticationTokenStore<UsersInfo> {

		private bool _disposed;

		public virtual Task AddClaimsAsync(UsersInfo user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken)) {
			ThrowIfDisposed();
			if (user == null) {
				throw new ArgumentNullException(nameof(user));
			}
			if (claims == null) {
				throw new ArgumentNullException(nameof(claims));
			}
			foreach (var claim in claims) {
				Userclaim.Insert(new UserclaimInfo {
					Create_time = DateTime.Now,
					Type = claim.Type,
					Value = claim.Value,
					Users_id = user.Id
				});
			}
			//System.Security.Authentication.ExtendedProtection.se
			return Task.FromResult(false);
		}

		public virtual Task AddLoginAsync(UsersInfo user, UserLoginInfo login, CancellationToken cancellationToken = default(CancellationToken)) {
			cancellationToken.ThrowIfCancellationRequested();
			ThrowIfDisposed();
			if (user == null) {
				throw new ArgumentNullException(nameof(user));
			}
			if (login == null) {
				throw new ArgumentNullException(nameof(login));
			}
			//ClaimTypes.
			//UserLogins.Add(CreateUserLogin(user, login));
			return Task.FromResult(false);
		}

		public Task AddToRoleAsync(UsersInfo user, string roleName, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<IdentityResult> CreateAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<IdentityResult> DeleteAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		protected void ThrowIfDisposed() {
			if (_disposed) {
				throw new ObjectDisposedException(GetType().Name);
			}
		}

		public void Dispose() {
			_disposed = true;
		}

		public Task<UsersInfo> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<UsersInfo> FindByIdAsync(string userId, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<UsersInfo> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<UsersInfo> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<int> GetAccessFailedCountAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<IList<Claim>> GetClaimsAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<string> GetEmailAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<bool> GetEmailConfirmedAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<bool> GetLockoutEnabledAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<DateTimeOffset?> GetLockoutEndDateAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<IList<UserLoginInfo>> GetLoginsAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<string> GetNormalizedEmailAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<string> GetNormalizedUserNameAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<string> GetPasswordHashAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<string> GetPhoneNumberAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<bool> GetPhoneNumberConfirmedAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<IList<string>> GetRolesAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<string> GetSecurityStampAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<string> GetTokenAsync(UsersInfo user, string loginProvider, string name, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<bool> GetTwoFactorEnabledAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<string> GetUserIdAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<string> GetUserNameAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<IList<UsersInfo>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<IList<UsersInfo>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<bool> HasPasswordAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<int> IncrementAccessFailedCountAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<bool> IsInRoleAsync(UsersInfo user, string roleName, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task RemoveClaimsAsync(UsersInfo user, IEnumerable<Claim> claims, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task RemoveFromRoleAsync(UsersInfo user, string roleName, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task RemoveLoginAsync(UsersInfo user, string loginProvider, string providerKey, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task RemoveTokenAsync(UsersInfo user, string loginProvider, string name, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task ReplaceClaimAsync(UsersInfo user, Claim claim, Claim newClaim, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task ResetAccessFailedCountAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetEmailAsync(UsersInfo user, string email, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetEmailConfirmedAsync(UsersInfo user, bool confirmed, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetLockoutEnabledAsync(UsersInfo user, bool enabled, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetLockoutEndDateAsync(UsersInfo user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetNormalizedEmailAsync(UsersInfo user, string normalizedEmail, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetNormalizedUserNameAsync(UsersInfo user, string normalizedName, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetPasswordHashAsync(UsersInfo user, string passwordHash, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetPhoneNumberAsync(UsersInfo user, string phoneNumber, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetPhoneNumberConfirmedAsync(UsersInfo user, bool confirmed, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetSecurityStampAsync(UsersInfo user, string stamp, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetTokenAsync(UsersInfo user, string loginProvider, string name, string value, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetTwoFactorEnabledAsync(UsersInfo user, bool enabled, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetUserNameAsync(UsersInfo user, string userName, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<IdentityResult> UpdateAsync(UsersInfo user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}
	}
}
