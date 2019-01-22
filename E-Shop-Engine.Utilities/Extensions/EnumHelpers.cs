using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace E_Shop_Engine.Utilities.Extensions
{
    public static class EnumHelpers
    {
        /// <summary>
        /// Retrieve display name from Display attribute.
        /// </summary>
        /// <param name="enumValue">Value.</param>
        /// <returns>Display name.</returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetName();
        }
    }
}