using System;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Xango.Mvc.AjaxAntiForgery
{
	internal sealed class AjaxAntiForgeryData
	{

		private const string AntiForgeryTokenFieldName = "__AjaxRequestVerificationToken";

		private const int TokenLength = 128 / 8;
		private readonly static RNGCryptoServiceProvider _prng = new RNGCryptoServiceProvider();

		private DateTime _creationDate = DateTime.UtcNow;
		private string _salt;
		private string _username;
		private string _value;

		public AjaxAntiForgeryData()
		{
		}

		// copy constructor
		public AjaxAntiForgeryData(AjaxAntiForgeryData token)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}

			CreationDate = token.CreationDate;
			Salt = token.Salt;
			Username = token.Username;
			Value = token.Value;
		}

		public DateTime CreationDate
		{
			get
			{
				return _creationDate;
			}
			set
			{
				_creationDate = value;
			}
		}

		public string Salt
		{
			get
			{
				return _salt ?? String.Empty;
			}
			set
			{
				_salt = value;
			}
		}

		public string Username
		{
			get
			{
				return _username ?? String.Empty;
			}
			set
			{
				_username = value;
			}
		}

		public string Value
		{
			get
			{
				return _value ?? String.Empty;
			}
			set
			{
				_value = value;
			}
		}

		private static string Base64EncodeForCookieName(string s)
		{
			byte[] rawBytes = Encoding.UTF8.GetBytes(s);
			string base64String = Convert.ToBase64String(rawBytes);

			// replace base64-specific characters with characters that are safe for a cookie name
			return base64String.Replace('+', '.').Replace('/', '-').Replace('=', '_');
		}

		private static string GenerateRandomTokenString()
		{
			byte[] tokenBytes = new byte[TokenLength];
			_prng.GetBytes(tokenBytes);

			string token = Convert.ToBase64String(tokenBytes);
			return token;
		}

		// If the app path is provided, we're generating a cookie name rather than a field name, and the cookie names should
		// be unique so that a development server cookie and an IIS cookie - both running on localhost - don't stomp on
		// each other.
		internal static string GetAntiForgeryTokenName(string appPath)
		{
			if (String.IsNullOrEmpty(appPath))
			{
				return AntiForgeryTokenFieldName;
			}
			else
			{
				return AntiForgeryTokenFieldName + "_" + Base64EncodeForCookieName(appPath);
			}
		}

		internal static string GetAntiForgeryTokenHeaderName()
		{
			return "X-Request-Verification-Token";
		}

		internal static string GetUsername(IPrincipal user)
		{
			if (user != null)
			{
				IIdentity identity = user.Identity;
				if (identity != null && identity.IsAuthenticated)
				{
					return identity.Name;
				}
			}

			return String.Empty;
		}

		public static AjaxAntiForgeryData NewToken()
		{
			string tokenString = GenerateRandomTokenString();
			return new AjaxAntiForgeryData()
			{
				Value = tokenString
			};
		}

	}
}