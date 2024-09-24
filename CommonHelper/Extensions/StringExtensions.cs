using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesageService.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveBearer(this string value)
        {
            StringBuilder sb = new StringBuilder(value);
            sb.Replace("bearer", string.Empty);
            sb.Replace("Bearer", string.Empty);
            sb.Replace(" ", string.Empty);
            return sb.ToString();

        }
    }
}
