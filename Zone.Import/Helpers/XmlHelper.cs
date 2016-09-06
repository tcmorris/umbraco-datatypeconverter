﻿namespace Zone.Import.Helpers
{
    public static class XmlHelper
    {
        public static bool IsValidXml(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                input = input.Trim();
                if (input.StartsWith("<") && input.EndsWith(">") && input.Contains("/"))
                    return true;
            }
            return false;
        }
    }
}