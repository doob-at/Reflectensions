﻿namespace doob.Reflectensions.Internal {
    internal static class InternalStringExtensions {

        public static string Repeat(this string value, int times)
        {
            return new StringBuilder().Insert(0, value, times).ToString();
        }

        //public static IEnumerable<string> SplitGenericArguments(this string value) {
        //    var parts = new List<string>();
        //    var parenLevel = 0;
        //    var lastPos = 0;
        //    for (var i = 0; i != value.Length; i++) {
        //        switch (value[i]) {
        //            case '<':
        //                parenLevel++;
        //                break;
        //            case '>':
        //                parenLevel--;
        //                if (parenLevel < 0) {
        //                    throw new ArgumentException();
        //                }
        //                break;
        //            case '[':
        //                if (value[i + 1] != ']') {
        //                    parenLevel++;
        //                }
        //                break;
        //            case ']': {
        //                    if (value[i - 1] != '[') {
        //                        parenLevel--;
        //                        if (parenLevel < 0) {
        //                            throw new ArgumentException();
        //                        }
        //                    }
        //                    break;
        //                }
        //            case ',':
        //                if (parenLevel == 0) {
        //                    parts.Add(value.Substring(lastPos, i - lastPos));
        //                    lastPos = i + 1;
        //                }
        //                break;
        //        }
        //    }
        //    if (lastPos != value.Length) {
        //        parts.Add(value.Substring(lastPos, value.Length - lastPos));
        //    }

        //    return parts.Select(p => p.Trim()).Where(p => !String.IsNullOrEmpty(p));
        //}

        public static string Trim(this string value, params string[] trimCharacters) {
            return value?.Trim(String.Join("", trimCharacters).ToCharArray()) ?? "";
        }
        
        public static string? TrimToNull(this string? value) {
            if (value == null)
                return null;

            return ToNull(value.Trim());
        }

        public static string? ToNull(this string value) {
            return String.IsNullOrEmpty(value) ? null : value;
        }
    }
}
