using System;
using System.Collections.Generic;
using System.Text;

namespace FirstShop.Core.Tools
{
    public class FixedText
    {
        public static string FixEmail(string email)
        {
            return email.Trim().ToLower();
        }
    }
}
