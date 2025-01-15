using System;
using System.Linq;

namespace Expressium.Coffeeshop.Web.API
{
    public class Randomizer
    {
        private static readonly Random random = new Random();

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int GetRandomInteger(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public static string GetUniqueId()
        {
            return "ID" + DateTime.Now.ToString("yyyyMMddHHmmssff");
        }

        public static string GetDateTimeId()
        {
            return DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
