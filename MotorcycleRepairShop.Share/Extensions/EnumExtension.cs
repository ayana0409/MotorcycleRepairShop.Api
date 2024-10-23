using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MotorcycleRepairShop.Share.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var type = enumValue.GetType();

            var member = type.GetMember(enumValue.ToString()).FirstOrDefault();
            var displayAttribute = member?.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.GetName() ?? enumValue.ToString();
        }
    }
}
