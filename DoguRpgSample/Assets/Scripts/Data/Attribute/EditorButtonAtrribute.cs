using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Data
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EditorButtonAttribute : PropertyAttribute
    {
        private Type type;
        private string methodName;

        public EditorButtonAttribute(Type type, string methodName)
        {
            this.type = type;
            this.methodName = methodName;
        }

        public void Invoke(Object obj)
        {
            var method = type.GetMethod(methodName,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            method?.Invoke(null, new object[] { obj });
        }
    }
}