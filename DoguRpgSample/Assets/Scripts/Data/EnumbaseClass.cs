using System;
using System.Collections.Generic;
using System.Linq;
using Data.Static;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Data
{
    [Serializable]
    public abstract class EnumbaseClass<TEnum>
        where TEnum : Enum
    {
        public abstract TEnum GetDataType();
        [SerializeField] public int id;
    }

    public abstract class EnumbaseClassRegistry<TEnum, TValue>
        where TEnum : Enum
        where TValue : EnumbaseClass<TEnum>
    {
        public abstract EnumbaseClassRegistry<TEnum, TValue> Instance();
        public abstract TValue Create(TEnum type);
    }

    public abstract class EnumbaseClassEntry<TEnum, TValue>
        where TEnum : Enum
        where TValue : EnumbaseClass<TEnum>
    {
        public TEnum type;
        [SerializeReference] public TValue value;

        public abstract TValue Create(TEnum type);

        public void CheckNull()
        {
            if (value == null || !value.GetDataType().Equals(type))
            {
                value = Create(type);
            }
        }
    }

    [Serializable]
    public abstract class EnumbaseScriptableObjectEntry<TEnum, TValue> : ScriptableObject
        where TEnum : Enum
        where TValue : EnumbaseClass<TEnum>
    {
        [SerializeField] public TEnum type;
        [SerializeReference] public TValue value;

        public abstract TValue Create(TEnum type);

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (!EditorConditions.IsValidatable()) return;
#endif
            if (value == null || !value.GetDataType().Equals(type))
            {
                value = Create(type);
            }
        }
    }

    public abstract class EnumbaseScriptableObjectList<TSelf, TEnum, TValue, TClass> : SingletonScriptableObject<TSelf>
        where TSelf : SingletonScriptableObject<TSelf>, new()
        where TEnum : Enum
        where TValue : EnumbaseClass<TEnum>
        where TClass : EnumbaseScriptableObjectEntry<TEnum, TValue>
    {
        [SerializeReference] public TClass[] entries = new TClass[]{};

        public abstract string GetResourceDirectoryPath();
        public abstract string GetTypeName();

        protected virtual void OnValidate()
        {
            Debug.Log($"{GetTypeName()}  OnValidate");

            if (!EditorConditions.IsValidatable()) return;
            
            entries = StaticInfoListUtil.LoadAssetsAndSort<TClass>($"Assets/Resources/{GetResourceDirectoryPath()}",
                (i) => i.value.id);
        }

        protected override void OnInit()
        {
            Debug.Log($"{GetTypeName()} entries count: {entries.Length}");
        }


        public sealed override string GetResourcePath()
        {
            return $"{GetResourceDirectoryPath()}/{GetTypeName()}";
        }

        public TClass GetEntry(int id)
        {
            return entries.First(i => i.value.id == id);
        }

        public TClass[] GetEntries(TEnum type)
        {
            return entries.Where(i => i.type.Equals(type)).ToArray();
        }
    }
}