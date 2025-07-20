using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CrossProject.Editor.OdinEntities
{
    public static class ReflectionUtility
    {
        public static Type FindType(string qualifiedTypeName)
        {
            Type t = Type.GetType(qualifiedTypeName);

            if (t != null)
                return t;
            
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies().Reverse())
            {
                t = asm.GetType(qualifiedTypeName);
                if (t != null)
                    return t;
            }
            return null;
        }
        
        public static bool IsGenericList(this Type oType) => 
            (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>)));

        public static void SetInstanceField(object obj, string fieldName, object value)
        {
            var fieldInfo = FindInstanceField(obj.GetType(), fieldName);
            fieldInfo.SetValue(obj, value);
        }

        public static object GetInstanceField(object obj, string fieldName)
        {
            var fieldInfo = FindInstanceField(obj.GetType(), fieldName);
            return fieldInfo.GetValue(obj);
        }

        public static FieldInfo FindInstanceField(Type type, string fieldName)
        {
            var t = type;
            while (t != null)
            {
                var field = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (field != null)
                {
                    return field;
                }

                t = t.BaseType;
            }

            Debug.LogError($"Field with name {fieldName} not found in type {type.Name}");
            return null;
        }
    }
}