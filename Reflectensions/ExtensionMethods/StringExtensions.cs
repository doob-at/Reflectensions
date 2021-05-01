using System;
using System.Collections.Generic;
using System.Text;

namespace Reflectensions.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string Repeat(this string value, int times)
        {
            return new StringBuilder().Insert(0, value, times).ToString();
        }

        public static string? TrimToNull(this string? value)
        {
            if (value == null)
                return null;

            return ToNull(value.Trim());
        }

        #region StringTo
        public static string? ToNull(this string? value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }

        #endregion
    }
}
