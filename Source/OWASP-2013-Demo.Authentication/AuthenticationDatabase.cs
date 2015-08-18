using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;

namespace OWASP_2013_Demo.Authentication
{
	public interface IAuthenticationDatabase
	{
		UserCredentials GetUserCredentialsByUsername(string username);

		void SaveUserCredentials(UserCredentials credentials);
	}

	public class UserCredentials
	{
		public string Username { get; set; }

		public string PasswordValidationString { get; set; }
	}

	public class InMemoryAuthenticationDatabase : IAuthenticationDatabase
	{
		private Dictionary<string,UserCredentials> credentialStore;

		private string backupFileLocation;

		public InMemoryAuthenticationDatabase()
		{
			credentialStore = new Dictionary<string, UserCredentials>();
			backupFileLocation = HttpContext.Current.Server.MapPath("~/App_Data/passwords.bak");
			
			if (!Directory.Exists(Path.GetDirectoryName(backupFileLocation)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(backupFileLocation));
			}

			if (File.Exists(backupFileLocation))
			{
				File.Delete(backupFileLocation);
			}
		}

		public UserCredentials GetUserCredentialsByUsername(string username)
		{
			if (credentialStore.ContainsKey(username.ToLower()))
			{
				return CopyCredentials(credentialStore[username.ToLower()]);
			}
			else
			{
				return null;
			}
		}

		public void SaveUserCredentials(UserCredentials credentials)
		{
			credentialStore[credentials.Username.ToLower()] = CopyCredentials(credentials);
			File.AppendAllText(backupFileLocation, credentials.Username + "\t" + credentials.PasswordValidationString + "\r\n");
		}

		private UserCredentials CopyCredentials(UserCredentials from)
		{
			Mapper.CreateMap<UserCredentials, UserCredentials>();
			return Mapper.Map<UserCredentials>(from);
		}
	}
}
