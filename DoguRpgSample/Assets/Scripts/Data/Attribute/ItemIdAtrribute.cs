using System;
using UnityEngine;

namespace Data
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ItemIdAttribute : PropertyAttribute
    {
    }
}