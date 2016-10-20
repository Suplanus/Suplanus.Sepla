using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Suplanus.Sepla.Extensions
{
   public static class StringExtensions
   {
      public static bool IsNullOrEmpty(this string s)
      {
         return string.IsNullOrEmpty(s);
      }
   }
}
