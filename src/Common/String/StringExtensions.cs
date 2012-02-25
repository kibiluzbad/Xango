using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Xango.Common.String
{
    public static class StringExtensions
    {
        public static string Slugify(this string phrase, int maxLength = 50)
        {
            return phrase.ToLower()
                .NonCharacterToEmptyString()
                .MultiSpacesToSingleSpace()
                .CutAndTrim(maxLength)
                .SpacesToHyphens();
        }

        public static string SpacesToHyphens(this string str)
        {
            str = Regex.Replace(str, @"\s", "-");
            return str;
        }

        public static string CutAndTrim(this string str, int maxLength)
        {
            return str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();
        }

        public static string MultiSpacesToSingleSpace(this string str)
        {
            return Regex.Replace(str, @"[\s-]+", " ").Trim();
        }

        public static string NonCharacterToEmptyString(this string str)
        {
            return Regex.Replace(str, @"[^a-z0-9\s-]", "");
        }
    }
}
