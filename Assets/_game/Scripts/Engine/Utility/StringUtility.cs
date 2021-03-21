using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RomenoCompany
{
    public static class StringUtility
    {
        static readonly Regex rePattern = new Regex(
            @"(\{+)([^\}]+)(\}+)", RegexOptions.Compiled);
        public static string Format(string pattern, object template)
        {
            if (template == null) throw new ArgumentNullException();
            Type type = template.GetType();
            var cache = new Dictionary<string, string>();
            return rePattern.Replace(pattern, match =>
            {
                int lCount = match.Groups[1].Value.Length,
                    rCount = match.Groups[3].Value.Length;
                if ((lCount % 2) != (rCount % 2)) throw new InvalidOperationException("Unbalanced braces");
                string lBrace = lCount == 1 ? "" : new string('{', lCount / 2),
                    rBrace = rCount == 1 ? "" : new string('}', rCount / 2);

                string key = match.Groups[2].Value, value;
                if(lCount % 2 == 0) {
                    value = key;
                } else {
                    if (!cache.TryGetValue(key, out value))
                    {
                        var prop = type.GetProperty(key);
                        if (prop == null)
                        {
                            throw new ArgumentException("Not found: " + key, "pattern");
                        }
                        value = Convert.ToString(prop.GetValue(template, null));
                        cache.Add(key, value);
                    }
                }
                return lBrace + value + rBrace;
            });
        }
    
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength); 
        }
    }    
}


