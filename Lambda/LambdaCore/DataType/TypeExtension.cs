using System;
using System.ComponentModel;
using System.Reflection;
using Lambda;

namespace LambdaManager.DataType
{
    public static class TypeExtension
    {
        public static string Description(this Type obj)
        {
            return GetDescription(obj);
        }

        public static string Description(this Severity obj)
        {
            return GetDescription(obj);
        }

        public static string GetDescription(object obj)
        {
            System.Type type = obj.GetType();
            MemberInfo[] infos = type.GetMember(obj.ToString() ?? "");
            if (infos != null && infos.Length != 0)
            {
                object[] attrs = infos[0].GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
                if (attrs != null && attrs.Length != 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return type.ToString();
        }
    }
}


