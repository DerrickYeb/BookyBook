using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public static class SD
    {
        public const string Proc_Covertype_Get = "usp_GetCoverType";
        public const string Proc_CoverType_Create = "usp_CreateType";
        public const string Proc_CoverType_Update = "usp_UpdateCoverType";
        public const string Proc_CoverType_Delete = "usp_DeleteCoverType";
        public const string Proc_CoverType_GetAll = "usp_GetCoverTypes";

        public const string Role_User_Indi = "Individual Customer";
        public const string Role_User_Comp = "Company Customer";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

        public const string ssShoppingCart = "Shopping Car Session";
        public static double GetPriceBasedOnQuantity(double quantity,double price,double price50,double price100)
        {
            if (quantity < 50)
            {
                return price;
            }
            else
            {
                if (quantity < 100)
                {
                    return price50;
                }
                else
                {
                    return price100;
                }
            }
        }

        public static string ConvertToRawHtml(string source)
        {
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i
                < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    (new Char[source.Length])[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(new Char[source.Length], 0, arrayIndex);
        }
    }
}
