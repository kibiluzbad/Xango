using System;
using System.Web;
using System.Web.Mvc;

namespace Xango.Mvc.AjaxAntiForgery
{
	public static class AjaxAntiForgeryHtmlExtensions
	{
		private static AjaxAntiForgeryDataSerializer _serializer;
		internal static AjaxAntiForgeryDataSerializer Serializer
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

		public static MvcHtmlString AjaxAntiForgeryToken(this HtmlHelper helper)
		{
			return AjaxAntiForgeryToken(helper, null /* salt */);
		}

		public static MvcHtmlString AjaxAntiForgeryToken(this HtmlHelper helper, string salt)
		{
			return AjaxAntiForgeryToken(helper, salt, null /* domain */, null /* path */);
		}

		public static MvcHtmlString AjaxAntiForgeryToken(this HtmlHelper helper, string salt, string domain, string path)
		{
			string formValue = GetAntiForgeryTokenAndSetCookie(helper, salt, domain, path);
			string fieldName = AjaxAntiForgeryData.GetAntiForgeryTokenName(null);

			TagBuilder builder = new TagBuilder("meta");
			builder.Attributes["name"] = fieldName;
			builder.Attributes["content"] = formValue;
			return MvcHtmlString.Create(builder.ToString(TagRenderMode.StartTag));
		}

		private static string GetAntiForgeryTokenAndSetCookie(this HtmlHelper helper, string salt, string domain, string path)
		{
			string cookieName = AjaxAntiForgeryData.GetAntiForgeryTokenName(helper.ViewContext.HttpContext.Request.ApplicationPath);

			AjaxAntiForgeryData cookieToken;
			HttpCookie cookie = helper.ViewContext.HttpContext.Request.Cookies[cookieName];
			if (cookie != null)
			{
				cookieToken = Serializer.Deserialize(cookie.Value);
			}
			else
			{
				cookieToken = AjaxAntiForgeryData.NewToken();
				string cookieValue = Serializer.Serialize(cookieToken);

				HttpCookie newCookie = new HttpCookie(cookieName, cookieValue) { HttpOnly = true, Domain = domain };
				if (!String.IsNullOrEmpty(path))
				{
					newCookie.Path = path;
				}
				helper.ViewContext.HttpContext.Response.Cookies.Set(newCookie);
			}

			AjaxAntiForgeryData formToken = new AjaxAntiForgeryData(cookieToken)
			{
				Salt = salt,
				Username = AjaxAntiForgeryData.GetUsername(helper.ViewContext.HttpContext.User)
			};
			string formValue = Serializer.Serialize(formToken);
			return formValue;
		}
	}
}