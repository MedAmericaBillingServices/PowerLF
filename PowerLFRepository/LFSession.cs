using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using Laserfiche.RepositoryAccess;
using PowerLFRepository.Data;

namespace PowerLFRepository
{
	public class LFSession
	{
		internal Session Session { get; set; }

		public string ServerName => Session.Repository.ServerName;

		public string RepositoryName => Session.Repository.Name;

		internal LFSession(string serverName, string repositoryName, string userName = null, string password = null)
		{
			Session = userName == null ? Session.Create(serverName, repositoryName) : Session.Create(serverName, repositoryName, userName, password);
			Session.ApplicationName = "PowerLF";
		}

		/// <summary>
		/// Creates an account in the Laserfiche Repository
		/// </summary>
		/// <param name="sid">SecurityIdentifier for AD Trustee to add</param>
		/// <param name="featureRightsList"></param>
		/// <returns></returns>
		internal LFTrustee AddTrustee(SecurityIdentifier sid, List<string> featureRights)
		{
			FeatureRights rights;

			if (Enum.TryParse(string.Join(", ", featureRights), out rights) == false)
			{
				throw new ArgumentException("Failed to parse Feature Rights", nameof(featureRights));
			}

			var trustee = new TrusteeInfo();
			trustee.FeatureRights = rights;

			Trustee.SetInfo(new AccountReference(sid, Session), trustee, Session);
			Session.Save();

			return new LFTrustee(Trustee.GetInfo(sid, Session));
		}


		/// <summary>
		/// Removes an account from the Laserfiche Repository
		/// </summary>
		/// <param name="identity">Active Directory principal to remove</param>
		internal void RemoveAccount(Principal identity)
		{
			Trustee.DeleteSecurity(identity.Sid, Session);

			Session.Save();
		}

		#region Trustee management

		internal void RemoveTrustee(SecurityIdentifier sid)
		{
			var info = Trustee.GetInfo(sid, Session);

			info.Delete();

			info.Save();

			Session.Save();
		}

		internal void DenyTrustee(Principal identity)
		{
			Repository.DenyLogOnAccess(identity.Sid, Session);
			Session.Save();
		}

		internal AccountInfo SetTrusteeAccess(Principal identity, FeatureRights featureRights, Privileges privileges)
		{
			var trustee = Trustee.GetInfo(identity.Sid, Session);

			trustee.Privileges = privileges;
			trustee.FeatureRights = featureRights;
			Session.Save();

			return Account.GetInfo(identity.Sid, Session);
		}

		internal List<LFTrustee> GetAllTrustees()
		{
			return Trustee.EnumAllWindowsAccounts(Session).AsEnumerable().Select(x => new LFTrustee(x)).ToList();
		}

		#endregion

		internal void SetUserAccess(string username, FeatureRights featureRights, Privileges privileges)
		{
			var trustee = Account.GetInfo(username, AccountInfo.AccountFields.All, Session);
			trustee.Privileges = privileges;
			trustee.FeatureRights = featureRights;
			Session.Save();
		}

		internal AccountInfo AddUser(string name, string password, bool canChangePassword, bool passwordNeverExpires, bool canUsePassword)
		{
			var user = new UserInfo()
			{
				Name = name,
				Password = password,
				CanChangePassword = canChangePassword,
				PasswordNeverExpires = passwordNeverExpires,
				CanUsePassword = canUsePassword
			};

			return Account.Create(user, false, Session);
		}

		internal void RemoveUser(AccountInfo userInfo)
		{
			userInfo.Delete();
			Session.Save();
		}

		internal AccountInfo GetUser(string username)
		{
			return Account.GetInfo(username, AccountInfo.AccountFields.All, Session);
		}

		internal AccountInfo GetUser(Principal principal)
		{
			return Account.GetInfo(principal.Sid, AccountInfo.AccountFields.All, Session);
		}

		internal List<AccountInfo> GetAllUsers()
		{
			return Account.EnumUsers(Session).AsEnumerable().ToList();
		}

		public LFTrustee GetTrustee(SecurityIdentifier sid)
		{
			return new LFTrustee(Trustee.GetInfo(sid, Session));
		}

		public int CreateVolume(string name, string fixedPath, bool rollover, long maxSizeMb)
		{
			var info = Volume.Create(new VolumeInfo
			{
				VolumeType = rollover ? VolumeType.Logical : VolumeType.Physical,
				FixedPath = fixedPath,
				Name = name,
				MaximumSize = maxSizeMb,

			}, Session);

			return info.Id;
		}
	}
}
