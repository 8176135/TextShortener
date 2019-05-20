using System;

namespace testingWebApp
{
    public static class RandomStringGen
    {
        public static string GenId(int len)
        {
            string guidString = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            guidString = guidString.Replace("=", "")
                .Replace("+", "")
                .Replace("/", "");
            return guidString.Substring(guidString.Length - len);
        }
    }
}