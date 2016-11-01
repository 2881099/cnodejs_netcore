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
	public partial class RoleStore : IRoleClaimStore<RolesInfo> {

		private bool _disposed;
		public Task AddClaimAsync(RolesInfo role, Claim claim, CancellationToken cancellationToken = default(CancellationToken)) {
			
			ThrowIfDisposed();
			if (role == null) {
				throw new ArgumentNullException(nameof(role));
			}
			if (claim == null) {
				throw new ArgumentNullException(nameof(claim));
			}

			Roleclaim.Insert(new RoleclaimInfo {
				Create_time = DateTime.Now,
				Roles_id = role.Id,
				Type = claim.Type,
				Value = claim.Value
			});
			return Task.FromResult(false);
		}

		public async virtual Task<IdentityResult> CreateAsync(RolesInfo role, CancellationToken cancellationToken = default(CancellationToken)) {
			cancellationToken.ThrowIfCancellationRequested();
			ThrowIfDisposed();
			if (role == null) {
				throw new ArgumentNullException(nameof(role));
			}
			bll.Roles.Insert(role);
			return IdentityResult.Success;
		}

		public async virtual Task<IdentityResult> DeleteAsync(RolesInfo role, CancellationToken cancellationToken = default(CancellationToken)) {
			cancellationToken.ThrowIfCancellationRequested();
			ThrowIfDisposed();
			if (role == null) {
				throw new ArgumentNullException(nameof(role));
			}
			role.UnflagUsersALL();
			bll.Roles.Delete(role.Id);
			return IdentityResult.Success;
		}

		public void Dispose() {
			_disposed = true;
		}
		protected void ThrowIfDisposed() {
			if (_disposed) {
				throw new ObjectDisposedException(GetType().Name);
			}
		}

		public virtual Task<RolesInfo> FindByIdAsync(string roleId, CancellationToken cancellationToken = default(CancellationToken)) {
			cancellationToken.ThrowIfCancellationRequested();
			ThrowIfDisposed();
			uint id;
			uint.TryParse(roleId, out id);
			var ret = bll.Roles.GetItem(id);
			return Task.FromResult(ret);
		}

		public Task<RolesInfo> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) {
			cancellationToken.ThrowIfCancellationRequested();
			ThrowIfDisposed();
			var ret = bll.Roles.GetItemByName(normalizedRoleName);
			return Task.FromResult(ret);
		}

		public async Task<IList<Claim>> GetClaimsAsync(RolesInfo role, CancellationToken cancellationToken = default(CancellationToken)) {
			ThrowIfDisposed();
			if (role == null) {
				throw new ArgumentNullException(nameof(role));
			}
			var cs = Roleclaim.SelectByRoles_id(role.Id).ToList();
			return cs.Select<RoleclaimInfo, Claim>(a => new Claim(a.Type, a.Value)).ToList();
		}

		public virtual Task<string> GetNormalizedRoleNameAsync(RolesInfo role, CancellationToken cancellationToken = default(CancellationToken)) {
			cancellationToken.ThrowIfCancellationRequested();
			ThrowIfDisposed();
			if (role == null) {
				throw new ArgumentNullException(nameof(role));
			}
			return Task.FromResult(role.Name);
		}

		public Task<string> GetRoleIdAsync(RolesInfo role, CancellationToken cancellationToken = default(CancellationToken)) {
			cancellationToken.ThrowIfCancellationRequested();
			ThrowIfDisposed();
			if (role == null) {
				throw new ArgumentNullException(nameof(role));
			}
			return Task.FromResult(string.Concat(role.Id));
		}

		public Task<string> GetRoleNameAsync(RolesInfo role, CancellationToken cancellationToken = default(CancellationToken)) {
			cancellationToken.ThrowIfCancellationRequested();
			ThrowIfDisposed();
			if (role == null) {
				throw new ArgumentNullException(nameof(role));
			}
			return Task.FromResult(role.Name);
		}

		public async Task RemoveClaimAsync(RolesInfo role, Claim claim, CancellationToken cancellationToken = default(CancellationToken)) {
			ThrowIfDisposed();
			if (role == null) {
				throw new ArgumentNullException(nameof(role));
			}
			if (claim == null) {
				throw new ArgumentNullException(nameof(claim));
			}
			var claims = Roleclaim.SelectByRoles_id(role.Id).WhereType(claim.Type).WhereValue(claim.Value).ToList();
			foreach (var c in claims) {
				Roleclaim.Delete(c.Id);
			}
		}

		public virtual Task SetNormalizedRoleNameAsync(RolesInfo role, string normalizedName, CancellationToken cancellationToken = default(CancellationToken)) {
			cancellationToken.ThrowIfCancellationRequested();
			ThrowIfDisposed();
			if (role == null) {
				throw new ArgumentNullException(nameof(role));
			}
			role.UpdateDiy.SetName(normalizedName).ExecuteNonQuery();
			return Task.CompletedTask;
		}

		public Task SetRoleNameAsync(RolesInfo role, string roleName, CancellationToken cancellationToken = default(CancellationToken)) {
			cancellationToken.ThrowIfCancellationRequested();
			ThrowIfDisposed();
			if (role == null) {
				throw new ArgumentNullException(nameof(role));
			}
			role.UpdateDiy.SetName(roleName).ExecuteNonQuery();
			return Task.CompletedTask;
		}

		public async virtual Task<IdentityResult> UpdateAsync(RolesInfo role, CancellationToken cancellationToken = default(CancellationToken)) {
			cancellationToken.ThrowIfCancellationRequested();
			ThrowIfDisposed();
			if (role == null) {
				throw new ArgumentNullException(nameof(role));
			}
			return IdentityResult.Success;
		}
	}
}
