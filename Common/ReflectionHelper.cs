using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ReflectionHelper
    {
        public static Object GetPropertyValue(this Object obj, String propertyName)
        {
            string[] nameParts = propertyName.Split('.');
            if (nameParts.Length == 1)
            {
                return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
            }

            foreach (String part in nameParts)
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static Type GetPropertyType(String name, Type type)
        {
            var parts = name.Split('.').ToList();
            var currentPart = parts[0];
            PropertyInfo info = type.GetProperty(currentPart);
            if (info == null) { return null; }
            if (name.IndexOf(".", StringComparison.Ordinal) > -1)
            {
                parts.Remove(currentPart);
                return GetPropertyType(String.Join(".", parts), info.PropertyType);
            }
            return info.PropertyType;
        }
    }
}
