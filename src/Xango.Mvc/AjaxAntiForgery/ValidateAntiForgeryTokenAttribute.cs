using System;
using System.Web;
using System.Web.Mvc;

namespace Xango.Mvc.AjaxAntiForgery
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class AjaxValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
	{

		private string _salt;
		private AjaxAntiForgeryDataSerializer _serializer;

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

		internal AjaxAntiForgeryDataSerializer Serializer
		{
			get
			{
				if (_serializer == null)
				{
					_serializer = new AjaxAntiForgeryDataSerializer();
				}
				return _serializer;
			}
			set
			{
				_serializer = value;
			}
		}

		private bool ValidateFormToken(AjaxAntiForgeryData token)
		{
			return (String.Equals(Salt, token.Salt, StringComparison.Ordinal));
		}

		private static HttpAntiForgeryException CreateValidationException()
		{
			return new HttpAntiForgeryException("A required anti-forgery token was not supplied or was invalid.");
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext == null)
			{
				throw new ArgumentNullException("filterContext");
			}

			string fieldName = AjaxAntiForgeryData.GetAntiForgeryTokenName(null);
			string cookieName = AjaxAntiForgeryData.GetAntiForgeryTokenName(filterContext.HttpContext.Request.ApplicationPath);

			HttpCookie cookie = filterContext.HttpContext.Request.Cookies[cookieName];
			if (cookie == null || String.IsNullOrEmpty(cookie.Value))
			{
				// error: cookie token is missing
				throw CreateValidationException();
			}
			AjaxAntiForgeryData cookieToken = Serializer.Deserialize(cookie.Value);

			string formValue = filterContext.HttpContext.Request.Headers[AjaxAntiForgeryData.GetAntiForgeryTokenHeaderName()];
			if (String.IsNullOrEmpty(formValue))
			{
				// error: form token is missing
				throw CreateValidationException();
			}
			AjaxAntiForgeryData formToken = Serializer.Deserialize(formValue);

			if (!String.Equals(cookieToken.Value, formToken.Value, StringComparison.Ordinal))
			{
				// error: form token does not match cookie token
				throw CreateValidationException();
			}

			string currentUsername = AjaxAntiForgeryData.GetUsername(filterContext.HttpContext.User);
			if (!String.Equals(formToken.Username, currentUsername, StringComparison.OrdinalIgnoreCase))
			{
				// error: form token is not valid for this user
				// (don't care about cookie token)
				throw CreateValidationException();
			}

			if (!ValidateFormToken(formToken))
			{
				// error: custom validation failed
				throw CreateValidationException();
			}
		}

	}
}